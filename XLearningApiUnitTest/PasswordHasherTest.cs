using System;
using System.Collections.Generic;
using System.Text;
using Helpers;
using Moq;
using Xunit;

namespace XLearningApiUnitTest
{
    public class PasswordHasherTest
    {
        private Mock<PasswordHasher> hasher;
        public PasswordHasherTest()
        {
            hasher = new Mock<PasswordHasher>();
          
        }
        [Fact]
        public void ShouldReturnHash()
        {
            //arrange 
            string dynamicSalt = "test";
            string password = "123";
            //act
           var hash1= hasher.Object.GenerateHashForTesting(password, dynamicSalt);
           var hash2= hasher.Object.GenerateHashForTesting(password, dynamicSalt);
            //assert

            Assert.Equal(hash1, hash2);
        }
        [Fact]
        public void ShouldVerifyHash()
        {
            //arrange
            string password = "123";
            string randomSalt = "%$#";
            //act
            var hash = hasher.Object.GenerateHashForTesting(password, randomSalt);
            var isSameHash = hasher.Object.IsHashMatchedForTesting(password, randomSalt, hash);
            //assert
            Assert.True(isSameHash);
        }
    }
}
