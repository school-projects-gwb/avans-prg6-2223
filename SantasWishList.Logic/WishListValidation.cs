using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantasWishList.Logic
{
    public class WishListValidation
    {
        public ValidationResult ValidateWishList()
        {
            ValidationResult result;
            result = ValidateAmountOfGifts();
            if (CheckError(result)) { return result; }

            result = ValidateAge();
            if (CheckError(result)) { return result; }

            result = ValidateLegoOrKnex();
            if (CheckError(result)) { return result; }

            result = ValidateNightLamp();
            if (CheckError(result)) { return result; }

            result = ValidateMusic();
            if (CheckError(result)) { return result; }

            result = ValidateUniqueGift();
            if (CheckError(result)) { return result; }

            result = ValidateCostumRule();
            return result;
        }

        private bool CheckError(ValidationResult check)
        {
            if(check == ValidationResult.Success) { return false; }
            return true;
        }

        public ValidationResult ValidateAmountOfGifts()
        {
            /*
             * instructions:
             * normally a kid is allowed to choose 3 gifts per catagory.
             * if a kid has been naughty but is honest they can choose 1 gift per catagory
             * if a kid has been naughty and lies about it they can choose 1 gift total
             * 
             * this one has to be combined with ValidateStijnDolfje
             * or should it be in this method as wel?
             * 
             * Maybe we should checkk both name and if someone does charity work here...
             */

            throw new NotImplementedException();
        }

        private bool CheckForCharityWork(string description)
        {
            /*
             * instructions:
             * Check if a kid used "vrijwilligerswerk" in their description.
             * if so return treu, if not return false
             * has to be a nice kid but that will be checked in the validator.
             */

            //put it all to lower case just to make sure that case sensitivity doesn't mess with this one.
            if (description.ToLower().Contains("vrijwilligerswerk")) { return true; }
            return false;
        }

        private bool CheckStijnDolfje()
        {
            /*
             * instructions:
             * if the name of the kid is "Stijn" and they chose for a
             * dolfje weerwolfje book, return true
             * 
             * else return false.
             */

            throw new NotImplementedException();
        }

        public ValidationResult ValidateLegoOrKnex()
        {
            /*
             * instructions:
             * A kid is not allowed to have both lego and knex on their wishlist
             */

            throw new NotImplementedException();
        }

        public ValidationResult ValidateAge()
        {
            /*
             * instructions:
             * A kid is allowed to have 1 gift with age classification above their age.
             * more than that has to return an error
             */

            throw new NotImplementedException();
        }

        public ValidationResult ValidateNightLamp()
        {
            /*
             * instructions:
             * if a kid asks for a nightlamp (nachtlamp) they also have to ask for
             * underwear (ondergoed)
             */

            throw new NotImplementedException();
        }

        public ValidationResult ValidateMusic()
        {
            /*
             * instructions:
             * if a kid asks for a music instrumet (muziekinstrument) they also have to ask for
             * earbuds (oordopjes)
             */

            throw new NotImplementedException();
        }

        public ValidationResult ValidateUniqueGift()
        {
            /*
             * instructions:
             * if a kid asks for a gift that is not in the list it must be checked to
             * make sure it is not in the list.
             */

            throw new NotImplementedException();
        }

        public ValidationResult ValidateCostumRule()
        {
            /*
             * instructions:
             * if a kid asks for a "choladeletter" or "pepernoten" return an error saying
             * "Ik ben toch zeker sinterklaas niet.
             */

            throw new NotImplementedException();
        }

    }
}
