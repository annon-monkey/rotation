using rotation.Common;
using rotation.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Entities
{
    class EntityTimeline
    {
        public IList<IEntity> Entities => this._entities;
        private readonly ObservableCollection<IEntity> _entities = new ObservableCollection<IEntity>();

        public IEnumerable<RotationDataRow> Rows => this._rowsCache = this._rowsCache ?? this.Entities.SelectMany(e => e.Output()).ToArray();
        private IEnumerable<RotationDataRow> _rowsCache;

        public Time LengthTime => new Time(this.Rows.Sum(r => r.DuringTime.Value));

        public EntityTimeline()
        {
            this._entities.CollectionChanged += (sender, e) => this._rowsCache = null;
        }

        public RotationDataRow GetRow(Time time)
        {
            var totalTime = 0;
            var rows = this.Rows;
            foreach (var row in rows)
            {
                totalTime += row.DuringTime.Value;

                if (totalTime > time.Value)
                {
                    return row;
                }
            }
            return rows.Last();
        }

        public RotationDataRow GetRow(int time) => this.GetRow(new Time(time));

        public IEnumerable<OutputDataRow> OrganizedData
        {
            get
            {
                IEnumerable<OutputDataRow> allRows()
                {
                    var totalTime = 0;
                    var rows = this.Rows;
                    var temp = new OutputDataRow();
                    foreach (var row in rows)
                    {
                        temp.Time = totalTime;
                        temp.Velocity = OutputUtil.ToOutputVelocity(row.Velocity);
                        yield return temp;

                        totalTime += row.DuringTime.Value;
                    }
                    yield return new OutputDataRow
                    {
                        Time = totalTime,
                        Velocity = 0,
                    };
                }

                OutputDataRow last = default;
                yield return new OutputDataRow
                {
                    Time = 0,
                    Velocity = 0,
                };
                foreach (var row in allRows())
                {
                    if (row.Velocity != last.Velocity)
                    {
                        //// 差が大きすぎる場合の補正処理
                        //if (Math.Abs(row.Velocity - last.Velocity) > 75)
                        //{
                        //    var velo = row.Velocity;
                        //    var delta = row.Velocity > last.Velocity ? -42 : 42;
                        //    var list = new List<OutputDataRow>();
                        //    for (var i = row.Time - 12; i > last.Time; i -= 12)
                        //    {
                        //        velo += delta;
                        //        if (row.Velocity > last.Velocity ? velo <= last.Velocity : velo >= last.Velocity)
                        //        {
                        //            break;
                        //        }
                        //        list.Add(new OutputDataRow
                        //        {
                        //            Time = i,
                        //            Velocity = velo,
                        //        });
                        //    }
                        //    foreach (var l in list.AsEnumerable().Reverse())
                        //    {
                        //        yield return l;
                        //    }
                        //}
                        yield return row;

                        last = row;
                    }
                }
            }
        }
    }
}
