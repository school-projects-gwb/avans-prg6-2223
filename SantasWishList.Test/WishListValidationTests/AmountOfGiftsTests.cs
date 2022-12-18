using SantasWishlist.Domain;
using SantasWishList.Logic.Validation;
using System.ComponentModel.DataAnnotations;
using SantasWishList.Web.Logic;

namespace SantasWishList.Test.WishListValidationTests
{
    public class AmountOfGiftsTests
    {
        private WishListValidator _validator = new WishListValidator(new GiftRepository());

        [Fact]
        public void SuccessfullWishlist()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.AddGiftsPerCatagory(child, 3);

            List<ValidationResult> errors = _validator.ValidateWishList(child);
            
            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));
        }

        [Fact]
        public void TooManyGiftsNice()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.AddGiftsPerCatagory(child, 3);

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "MTG kaarten", GiftCategory.WANT);
            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Je mag per catogory maar 3 cadeautjes uitzoeken.", errors));
        }

        [Fact]
        public void TooManyGiftsNaughtyHonest()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, false, Behaviour.STOUT);
            child = HelpFunctionsTests.AddGiftsPerCatagory(child, 1);

            List<ValidationResult> errors = _validator.ValidateWishList(child );

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "MTG kaarten", GiftCategory.NEED);
            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Jij bent stout maar eerlijk. Maar je mag maar 1 cadeautje per catagory kiezen.", errors));

            child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, false, Behaviour.BEETJE);
            child = HelpFunctionsTests.AddGiftsPerCatagory(child, 1);

            errors = _validator.ValidateWishList(child );

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "MTG kaarten", GiftCategory.NEED);
            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Jij bent stout maar eerlijk. Maar je mag maar 1 cadeautje per catagory kiezen.", errors));
        }

        [Fact]
        public void TooManyGiftsNaughtyLiar()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, false, Behaviour.BRAAF);
            child = HelpFunctionsTests.addCostumGift(child, "MTG kaarten", GiftCategory.WANT);

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "een draak", GiftCategory.NEED);

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Jij bent stout geweest en liegt er om! Jij mag maar 1 cadeautje kiezen.", errors));
        }

        [Fact]
        public void InfiniteGiftsForCharityWorkers()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer, die aan vrijwilligerswerk doet.", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addCostumGift(child, "MTG kaarten", GiftCategory.WANT);
            List<ValidationResult> errors;

            for (int i = 0; i < 3; i++)
            {
                child = HelpFunctionsTests.AddGiftsPerCatagory(child, 2);
                errors = _validator.ValidateWishList(child );
                Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));
            }

            child = HelpFunctionsTests.makeChild("Daan", "Een gamer, die aan vrijwilligerswerk doet.", 20, false, Behaviour.BRAAF);
            child = HelpFunctionsTests.AddGiftsPerCatagory(child, 1);

            errors = _validator.ValidateWishList(child );

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Jij bent stout geweest en liegt er om! Jij mag maar 1 cadeautje kiezen.", errors));
        }

    }
}
