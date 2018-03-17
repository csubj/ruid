using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            //List<RUID> sorted = Stream.generate(RUID::generate)
            //        .limit(30)
            //        .sorted()
            //        .collect(toList());

            //List<RUID> shuffled = new ArrayList<>(sorted);
            //Collections.shuffle(shuffled);
            //Collections.sort(shuffled);

            //assertEquals(sorted, shuffled);
        }

        [Fact]
        public void Json()
        {

            RUID original = RUID.Generate();
            String json = JsonConvert.SerializeObject(original);
            Console.WriteLine(json);
            RUID deserialized = JsonConvert.DeserializeObject<RUID>(json);
		    Console.WriteLine(deserialized);

            Assert.Equal(original, deserialized);
    }
}
}

