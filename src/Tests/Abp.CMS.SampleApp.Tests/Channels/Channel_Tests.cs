using Shouldly;
using Xunit;

namespace Abp.CMS.SampleApp.Tests.Channel
{
    public class Channel_Tests
    {
        [Fact]
        public void Test_CreateCode()
        {
            Channels.Channel.CreateCode().ShouldBe(null);
            Channels.Channel.CreateCode(42).ShouldBe("00042");
            Channels.Channel.CreateCode(1, 1, 3).ShouldBe("00001.00001.00003");
        }

        [Fact]
        public void Test_AppendCode()
        {
            Channels.Channel.AppendCode(null, "00005").ShouldBe("00005");
            Channels.Channel.AppendCode("00042", "00034").ShouldBe("00042.00034");
        }

        [Fact]
        public void Test_GetRelativeCode()
        {
            Channels.Channel.GetRelativeCode("00042", null).ShouldBe("00042");
            Channels.Channel.GetRelativeCode("00019.00055.00001", "00019").ShouldBe("00055.00001");
        }

        [Fact]
        public void Test_CalculateNextCode()
        {
            Channels.Channel.CalculateNextCode("00019.00055.00001").ShouldBe("00019.00055.00002");
            Channels.Channel.CalculateNextCode("00009").ShouldBe("00010");
        }

        [Fact]
        public void Test_GetLastUnitCode()
        {
            Channels.Channel.GetLastUnitCode("00055").ShouldBe("00055");
            Channels.Channel.GetLastUnitCode("00019.00055.00001").ShouldBe("00001");
        }

        [Fact]
        public void Test_GetParentCode()
        {
            Channels.Channel.GetParentCode("00055").ShouldBe(null);
            Channels.Channel.GetParentCode("00019.00055.00001").ShouldBe("00019.00055");
        }
    }
}