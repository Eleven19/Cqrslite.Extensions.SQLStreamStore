using System;
using System.Collections.Generic;
using System.Text;
using Specify.Stories;
using TestStack.BDDfy.Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CQRSlite.Extensions.SQLStreamStore.Testing
{
    public abstract class ScenarioFor<TSut, TStory> : Specify.ScenarioFor<TSut, TStory>
        where TSut : class
        where TStory : Story, new()
    {
        protected ScenarioFor(ITestOutputHelper output = null)
        {
            if (output?.GetType() == typeof(TestOutputHelper))
            {
                Output = output;
            }
        }
        protected static ITestOutputHelper Output { get; set; }


        [BddfyFact]
        public override void Specify()
        {
            base.Specify();
        }
    }

    public abstract class ScenarioFor<TSut> : Specify.ScenarioFor<TSut>
        where TSut : class
    {
        protected ScenarioFor(ITestOutputHelper output = null)
        {
            if (output?.GetType() == typeof(TestOutputHelper))
            {
                Output = output;
            }
        }
        protected static ITestOutputHelper Output { get; set; }

        [BddfyFact]
        public override void Specify()
        {
            base.Specify();
        }
    }
}
