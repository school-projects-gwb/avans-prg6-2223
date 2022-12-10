using SantasWishlist.Domain;
using SantasWishList.Data.Models;
using SantasWishList.Logic.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace SantasWishList.Test.WishListValidationTests
{
    public class TestingGiftSpecificRules
    {
        private WishListValidator _validator = new WishListValidator(new GiftRepository());

        [Fact]
        public void LegoAndKnex()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, "K`NEX");

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addSpecificGift(child, "LeGo");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Je mag niet om beide Lego en K`nex vragen", errors));

            child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, "LEgO");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addSpecificGift(child, "k`nex");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Je mag niet om beide Lego en K`nex vragen", errors));
        }

        [Fact]
        public void testAge()
        {
            /*
             * important notes:
             * highest possible age sees to be 12
             * this is the list gifts 12 and up:
             * spelcomputer
             * usb kabel
             */
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 10, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, "spelcomputer");

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addSpecificGift(child, "usb kabel");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Je mag maar een cadeautje kiezen waar je te jong voor bent", errors));
        }

        [Fact]
        public void StijnDolfje()
        {
            Child child = HelpFunctionsTests.makeChild("Stijn", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.AddGiftsPerCatagory(child, 3);

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addSpecificGift(child, "dolfje weerwolfje");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));
        }

        [Fact]
        public void NightLampWithUnderwear()
        {
            Assert.Equal("Als je om een nachtlampje vraagt moet je ook om ondergoed vragen.", TestCombinations("ondergoed", "nachtlampje"));
        }

        [Fact]
        public void InstrumentWithEarbuds()
        {
            Assert.Equal("Als je een instrument vraagt moet je ook oordropjes vragen", TestCombinations("oordopjes", "muziekinstrument"));
        }

        private string TestCombinations(string solo, string combined)
        {
            string errorMessage = null;
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, solo);

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            if(HelpFunctionsTests.countSuccesses(errors) != 7) { return null; }

            child = HelpFunctionsTests.addSpecificGift(child, combined);

            errors = _validator.ValidateWishList(child);

            if (HelpFunctionsTests.countSuccesses(errors) != 7) { return null; }

            child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, combined);

            errors = _validator.ValidateWishList(child);

            if (HelpFunctionsTests.countSuccesses(errors) != 6) { return null; }
            foreach(ValidationResult result in errors)
            {
                if(result != ValidationResult.Success)
                {
                    errorMessage = result.ErrorMessage;
                }
            }

            child = HelpFunctionsTests.addSpecificGift(child, solo);

            errors = _validator.ValidateWishList(child);

            if (HelpFunctionsTests.countSuccesses(errors) != 7) { return null; }
            return errorMessage;
        }

        [Fact]
        public void FillInOptionFromList()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, "lego");

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "lego", GiftCategory.WEAR);

            child.AdditionalGiftNames.Add("Lego");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Waar je extra om vroeg staat al in de lijst.", errors));
        }

        [Fact]
        public void Sinterklaas()
        {
            Child child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, "lego");

            List<ValidationResult> errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "chocoladeletter", GiftCategory.NEED);

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Ik ben toch zeker Sinterklaas niet.", errors));

            child = HelpFunctionsTests.makeChild("Daan", "Een gamer", 20, true, Behaviour.BRAAF);
            child = HelpFunctionsTests.addSpecificGift(child, "lego");

            errors = _validator.ValidateWishList(child);

            Assert.Equal(7, HelpFunctionsTests.countSuccesses(errors));

            child = HelpFunctionsTests.addCostumGift(child, "pepernoten", GiftCategory.NEED);

            errors = _validator.ValidateWishList(child);

            Assert.Equal(6, HelpFunctionsTests.countSuccesses(errors));
            Assert.True(HelpFunctionsTests.searchError("Ik ben toch zeker Sinterklaas niet.", errors));
        }

    }
}
