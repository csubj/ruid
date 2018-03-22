using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Csubj.Test
{
    public class RUIDTest
    { 

        [Fact]
        public void Parse()
        {
            RUID gen = RUID.Generate();
            Console.WriteLine(gen.ToString());
            Assert.Equal(gen, RUID.Parse(gen.ToString()));

            Assert.Equal(gen.GetHashCode(), new RUID(gen.Bytes()).GetHashCode());

            var sorted = new List<RUID>();
            for(int i = 0;i < 30;i++)
                sorted.Add(RUID.Generate());

            sorted.Sort();

            List<RUID> shuffled = new List<RUID>(sorted);
            shuffled = shuffled.OrderBy(a => Guid.NewGuid()).ToList();
            shuffled.Sort();

            Assert.Equal(sorted, shuffled);
        }

        [Fact]
        public void Json()
        {

            RUID original = RUID.Generate();
            String json = JsonConvert.SerializeObject(original);
            RUID deserialized = JsonConvert.DeserializeObject<RUID>(json);
            Assert.Equal(original, deserialized);
    }
}
}

