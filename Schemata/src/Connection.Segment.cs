using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public partial class Connection
{
    public partial class Segment
    {
        private Stack<Action> Callbacks { get; } = new();

        public Model Model { get; }

        public Segment(Model model)
        {
            Model = model;
        }

        public void Push(Connection connection)
        {
            connection.OnProcessing();
            Callbacks.Push(connection.OnProcessed);
        }
    }

    public partial class Segment : IDisposable
    {
        public void Dispose()
        {
            while (Callbacks.Any())
            {
                Callbacks.Pop().Invoke();
            }
        }
    }
}
