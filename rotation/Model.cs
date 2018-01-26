using rotation.Common;
using rotation.Data;
using rotation.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation
{
    class Model
    {
        private EntityTimeline timeline;

        public void Do()
        {
            this.MakeData();

            var writer = new AsyncConsoleWriter();

            var lastTime = default(int);
            using (var stream = new StreamWriter(@".\test.txt"))
            {
                foreach (var d in this.timeline.OrganizedData)
                {
                    stream.WriteLine(d.ToString());

                    writer.TryWrite($"\r 合計時間 : {d.Time}");
                    lastTime = d.Time;
                }
            }

            var span = new TimeSpan((long)lastTime / 10 * 10_000_000);
            if (span.Hours > 0 || span.Minutes >= 10)
            {
                Console.Write($"\n\n 出力完了 : {span.Hours} 時間 {span.Minutes} 分 {span.Seconds} 秒");
                Console.ReadKey();
            }
        }

        public void MakeData()
        {
            this.timeline = EntityBuilder.Start()
                .Point(80, 0.5)
                .Stop(5200)
                .Mutual(e => e.Point(6000, 0.15)
                              .Line(4000, 0.30)
                              .Point(3500, 0.15)
                              .Line(3000, 0.30)
                              .Loop(ee => ee.Stop(Time.Rand(10, 24))
                                            .Point(120, 0.15)
                                    , 24)
                              .Point(3500, 0.15)
                              .Loop(ee => ee.Stop(Time.Rand(10, 24))
                                            .Point(20, 0.15)
                                            .Line(120, 0.30)
                                    , 12)
                              .From(0.15)
                              .Line(3500, 0.36)
                              .Loop(ee => ee.Point(30, 0.30)
                                            .Trigonometric(125, 0.10, TrigonometricFunction.Start)
                                    , 22)
                              .Random(0.15, 0.22, 15, 200)
                              .Point(2500, 0.15)
                              .Line(4500, 0.45))
                .ToTimeline();
        }
    }

    /// <summary>
    /// コンソールへの描画を非同期スレッド上でおこなうことで
    /// 元の処理の時間を節約する。
    /// そのかわり、いくつか表示が省略されることがある
    /// </summary>
    class AsyncConsoleWriter
    {
        private string nextText;
        private bool isChanged = false;
        
        public AsyncConsoleWriter()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    lock (this)
                    {
                        if (this.isChanged)
                        {
                            Console.Write(this.nextText);
                            this.isChanged = false;
                        }
                    }
                }
            });
        }

        public void TryWrite(string text)
        {
            lock (this)
            {
                this.nextText = text;
                this.isChanged = true;
            }
        }
    }
}
