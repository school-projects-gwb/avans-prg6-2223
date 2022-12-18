using SantasWishlist.Domain;
using System.ComponentModel.DataAnnotations;
using SantasWishList.Web.Logic;

namespace SantasWishList.Logic.Validation
{
    public class WishListValidator {

        private IGiftRepository repo;

        public WishListValidator(IGiftRepository repo)
        {
            this.repo = repo;
        }
        public List<ValidationResult> ValidateWishList(Child child)
        {
            
            List<ValidationResult> results = new List<ValidationResult>();

            results.Add(ValidateAmountOfGifts(child));
            results.Add(ValidateAge(child));
            results.Add(ValidateLegoOrKnex(child.Wishlist));
            results.Add(ValidateNightLamp(child.Wishlist));
            results.Add(ValidateMusic(child.Wishlist));
            results.Add(ValidateUniqueGift(child.AdditionalGiftNames));
            results.Add(ValidateCostumRule(child.Wishlist));

            return results;
        }

        private ValidationResult ValidateAmountOfGifts(Child child)
        {
            /*
             * instructions:
             * normally a kid is allowed to choose 3 gifts per catagory.
             * if a kid has been nice and does charity work they can choose infinite gifts
             * if a kid has been naughty but is honest they can choose 1 gift per catagory
             * if a kid has been naughty and lies about it they can choose 1 gift total
             * 
             */
            
            //nice
            if (child.IsNaughty)
            {
                if (CheckForCharityWork(child.Reasoning)) { return ValidationResult.Success; }
                WishList wishlist = child.Wishlist;
                if (CheckStijnDolfje(child))
                {
                    Gift dolfje = null;
                    foreach(Gift gift in child.Wishlist.Wanted)
                    {
                        if (!gift.Name.ToLower().Equals("dolfje weerwolfje")) continue;
                        dolfje = gift; break;
                    }

                    if (dolfje != null) wishlist.Wanted.Remove(dolfje);
                }
                else if (!CheckAmountOfGiftsPerCatagory(wishlist, 3))
                {
                    return new ValidationResult("Je mag per catogory maar 3 cadeautjes uitzoeken.");
                }
            }
            else //naughty
            {
                if(child.Behaviour == Behaviour.BRAAF)
                    if(child.Wishlist.Wanted.Count() > 1) 
                        return new ValidationResult("Jij bent stout geweest en liegt er om! Jij mag maar 1 cadeautje kiezen.");

                if (!CheckAmountOfGiftsPerCatagory(child.Wishlist, 1)) 
                    return new ValidationResult("Jij bent stout maar eerlijk. Maar je mag maar 1 cadeautje per catagory kiezen.");
            }
            
            return ValidationResult.Success;
        }

        private bool CheckAmountOfGiftsPerCatagory(WishList wishlist, int amount)
        {
            Dictionary<GiftCategory, int> giftcount = new Dictionary<GiftCategory, int>();

            foreach(Gift gift in wishlist.Wanted)
            {
                try
                {
                    giftcount[gift.Category]++;
                }
                catch (KeyNotFoundException ex)
                {
                    giftcount.Add(gift.Category, 1);
                }
            }

            foreach (KeyValuePair<GiftCategory, int> entry in giftcount)
                if(entry.Value > amount) return false;

            return true;
        }

        private bool CheckForCharityWork(string? description)
        {
            if (description == null) return true;
            /*
             * instructions:
             * Check if a kid used "vrijwilligerswerk" in their description.
             * if so return true, if not return false
             * has to be a nice kid but that will be checked in the validator.
             */

            //put it all to lower case just to make sure that case sensitivity doesn't mess with this one.
            return description.ToLower().Contains("vrijwilligerswerk");
        }

        private bool CheckStijnDolfje(Child child)
        {
            /*
             * instructions:
             * if the name of the kid is "Stijn" and they chose for a
             * dolfje weerwolfje book, return true
             * 
             * else return false.
             * 
             * with this rule I assumed they have to be nice
             */
            if (!child.Name.ToLower().Equals("stijn")) return false;
            
            foreach(Gift gift in child.Wishlist.Wanted)
                if(gift.Name.Equals("Dolfje Weerwolfje")) return true;
            
            return false;
        }

        private ValidationResult ValidateLegoOrKnex(WishList wishlist)
        {
            /*
             * instructions:
             * A kid is not allowed to have both lego and knex on their wishlist
             */
            bool lego = false;
            bool knex = false;
            foreach(Gift gift in wishlist.Wanted)
            {
                if (gift.Name.ToLower().Equals("lego") && !lego) { lego = true;  }
                if (gift.Name.ToLower().Equals("k`nex") && !knex) { knex = true; }
                if(lego && knex) { return new ValidationResult("Je mag niet om beide Lego en K`nex vragen"); }
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateAge(Child child)
        {
            /*
             * instructions:
             * A kid is allowed to have 1 gift with age classification above their age.
             * more than that has to return an error
             */
            int GiftsTooYoung = 0;
            foreach(Gift gift in child.Wishlist.Wanted)
            {
                if(repo.CheckAge(gift.Name) > child.Age) { GiftsTooYoung++; }
                if (GiftsTooYoung > 1) { return new ValidationResult("Je mag maar een cadeautje kiezen waar je te jong voor bent"); }
            }
            
            return ValidationResult.Success;
        }

        private ValidationResult ValidateNightLamp(WishList wishlist)
        {
            /*
             * instructions:
             * if a kid asks for a nightlamp (nachtlamp) they also have to ask for
             * underwear (ondergoed)
             */
            if(CheckCombination("ondergoed", "nachtlampje", wishlist)) 
                return new ValidationResult("Als je om een nachtlampje vraagt moet je ook om ondergoed vragen.");
            
            return ValidationResult.Success;
        }

        private ValidationResult ValidateMusic(WishList wishlist)
        {
            /*
             * instructions:
             * if a kid asks for a music instrumet (muziekinstrument) they also have to ask for
             * earbuds (oordopjes)
             */
            return CheckCombination("oordopjes", "muziekinstrument", wishlist) ? 
                new ValidationResult("Als je een instrument vraagt moet je ook oordropjes vragen") : 
                ValidationResult.Success;
        }

        private bool CheckCombination(string standalone, string combined, WishList wishlist)
        {
            //in this method the standalone is the one that can be asked without the combined, not the other way around.
            bool alone = false;
            bool together = false;
            foreach (Gift gift in wishlist.Wanted)
            {
                if (gift.Name.ToLower().Equals(standalone)) alone = true;
                if (gift.Name.ToLower().Equals(combined)) together = true;
            }

            return together && !alone;
        }

        private ValidationResult ValidateUniqueGift(List<string>? customWishes)
        {
            /*
             * instructions:
             * if a kid asks for a gift that is not in the list it must be checked to
             * make sure it is not in the list.
             */

            if (customWishes == null || !customWishes.Any()) return ValidationResult.Success;
            
            List<Gift> options = repo.GetPossibleGifts();

            foreach (string asked in customWishes)
                foreach (Gift option in options)
                    if (asked.ToLower().Equals(option.Name.ToLower())) 
                        return new ValidationResult("Waar je extra om vroeg staat al in de lijst.");
            
            return ValidationResult.Success;
        }

        private ValidationResult ValidateCostumRule(WishList wishlist)
        {
            /*
             * instructions:
             * if a kid asks for a "choladeletter" or "pepernoten" return an error saying
             * "Ik ben toch zeker sinterklaas niet."
             */
            foreach(Gift gift in wishlist.Wanted)
                if(gift.Name.ToLower().Equals("pepernoten") || gift.Name.ToLower().Equals("chocoladeletter"))
                    return new ValidationResult("Ik ben toch zeker Sinterklaas niet.");
            
            return ValidationResult.Success;
        }

    }
}
