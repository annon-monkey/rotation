using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Common;
using rotation.Data;

namespace rotation.Entities
{
    class EntityBuilder : IEntity
    {
        double IEntity.LastVelocity => 0;

        IEnumerable<RotationDataRow> IEntity.Output() => Enumerable.Empty<RotationDataRow>();

        public static IEntity Start() => new EntityBuilder();
    }

    static class EntityBuilderExtensions
    {
        public static IEntity Point(this IEntity before, Time during, double velocity)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new PointEntity
            {
                DuringTime = during,
                Velocity = velocity,
            });
            return g;
        }
        public static IEntity Point(this IEntity before, Time during) => Point(before, during, before.LastVelocity);

        public static IEntity Await(this IEntity before)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new PointEntity
            {
                DuringTime = new Time(1),
                Velocity = before.LastVelocity,
            });
            return g;
        }

        public static IEntity From(this IEntity before, double from)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new FromEntity
            {
                FromVelocity = from,
            });
            return g;
        }

        public static IEntity Stop(this IEntity before, Time during)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new PointEntity
            {
                DuringTime = during,
                Velocity = 0,
            });
            return g;
        }

        public static IEntity Line(this IEntity before, Time during, double to)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new LineEntity
            {
                DuringTime = during,
                StartVelocity = before.LastVelocity,
                EndVelocity = to,
            });
            return g;
        }

        public static IEntity Mutual(this IEntity before, IEnumerable<IEntity> inner)
            => Mutual(before, inner, new MutualEntity().ReverseFunction);
        public static IEntity Mutual(this IEntity before, IEnumerable<IEntity> inner, IReverseFunction reverseFunction)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            var e = new MutualEntity
            {
                ReverseFunction = reverseFunction,
            };
            foreach (var ee in inner)
            {
                e.Timeline.Entities.Add(ee);
            }
            g.Entities.Add(e);
            return g;
        }
        public static IEntity Mutual(this IEntity before, Func<IEntity, IEnumerable<IEntity>> inner)
            => Mutual(before, inner(new EntityBuilder()));
        public static IEntity Mutual(this IEntity before, Func<IEntity, IEnumerable<IEntity>> inner, IReverseFunction reverseFunction)
            => Mutual(before, inner(new EntityBuilder()), reverseFunction);
        public static IEntity Mutual(this IEntity before, IEntity inner, IReverseFunction reverseFunction)
        {
            if (inner is EntityGroup g)
            {
                return Mutual(before, g.Entities, reverseFunction);
            }
            else
            {
                return Mutual(before, new IEntity[] { inner, }, reverseFunction);
            }
        }
        public static IEntity Mutual(this IEntity before, IEntity inner)
            => Mutual(before, inner, new MutualEntity().ReverseFunction);
        public static IEntity Mutual(this IEntity before, Func<IEntity, IEntity> inner)
            => Mutual(before, inner(new EntityBuilder()));
        public static IEntity Mutual(this IEntity before, Func<IEntity, IEntity> inner, IReverseFunction reverseFunction)
            => Mutual(before, inner(new EntityBuilder()), reverseFunction);

        public static IEntity Loop(this IEntity before, IEnumerable<IEntity> inner, int loopCount)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            var e = new LoopEntity
            {
                LoopCount = loopCount,
            };
            foreach (var ee in inner)
            {
                e.Entities.Add(ee);
            }
            g.Entities.Add(e);
            return g;
        }
        public static IEntity Loop(this IEntity before, Func<IEntity, IEnumerable<IEntity>> inner, int loopCount)
            => Loop(before, inner(new EntityBuilder()), loopCount);
        public static IEntity Loop(this IEntity before, IEntity inner, int loopCount)
        {
            if (inner is EntityGroup g)
            {
                return Loop(before, g.Entities, loopCount);
            }
            else
            {
                return Loop(before, new IEntity[] { inner, }, loopCount);
            }
        }
        public static IEntity Loop(this IEntity before, Func<IEntity, IEntity> inner, int loopCount)
            => Loop(before, inner(new EntityBuilder()), loopCount);

        public static IEntity Random(this IEntity before, double min, double max, Time singleTime, int loopCount)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new RandomEntity
            {
                LoopCount = loopCount,
                MaxVelocity = max,
                MinVelocity = min,
                SingleDuringTime = singleTime,
            });
            return g;
        }

        public static IEntity Trigonometric(this IEntity before, Time during, double to, TrigonometricFunction function)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(new TrigonometricEntity
            {
                DuringTime = during,
                StartVelocity = before.LastVelocity,
                EndVelocity = to,
                Function = function,
            });
            return g;
        }

        public static IEntity Func(this IEntity before, IEntity func)
        {
            var g = before as EntityGroup ?? new EntityGroup();
            g.Entities.Add(func);
            return g;
        }

        public static EntityTimeline ToTimeline(this IEntity entities)
        {
            var timeline = new EntityTimeline();

            if (entities is EntityGroup g)
            {
                foreach (var e in g.Entities)
                {
                    timeline.Entities.Add(e);
                }
            }
            else
            {
                timeline.Entities.Add(entities);
            }

            return timeline;
        }

        private class EntityGroup : IEntity
        {
            public List<IEntity> Entities { get; } = new List<IEntity>();

            public double LastVelocity => this.Entities.LastOrDefault()?.LastVelocity ?? 0;

            public IEnumerable<RotationDataRow> Output()
            {
                return this.Entities.SelectMany(e => e.Output());
            }
        }

        private class FromEntity : IEntity
        {
            public double FromVelocity { get; set; }

            public double LastVelocity => this.FromVelocity;

            public IEnumerable<RotationDataRow> Output()
            {
                return Enumerable.Empty<RotationDataRow>();
            }
        }
    }
}
