using Microsoft.AspNetCore.Mvc.Rendering;
using SantasWishlist.Domain;
using SantasWishList.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantasWishList.Test.WishListValidationTests
{
    public static class HelpFunctionsTests
    {
        public static Child makeChild(string name, string description, int age, bool santaJudgement, Behaviour behaviour)
        {
            Child child = new Child();
            child.IsNaughty = santaJudgement;
            child.Wishlist = new WishList();
            child.Wishlist.Wanted = new List<Gift>();
            child.Age = age;
            child.Behaviour = behaviour;
            child.Reasoning = description;

            return child;
        }

        public static Child AddGiftsPerCatagory(Child child, int amount)
        {
            IGiftRepository repo = new GiftRepository();
            List<Gift> gifts = repo.GetPossibleGifts();
            List<string> bannedGifts = new List<string>();
            bannedGifts.Add("Lego");
            bannedGifts.Add("Muziekinstrument");
            bannedGifts.Add("nachtlampje");

            int wantCount = 0, readCount = 0, wearCount = 0, needCount = 0;
            
            foreach (Gift gift in gifts)
            {
                bool alreadyOnList = false;
                foreach(Gift onlist in child.Wishlist.Wanted)
                {
                    if (onlist.Name.Equals(gift.Name))
                    {
                        alreadyOnList = true;
                        break;
                    }
                }

                bool giftbanned = false;
                foreach (string bannedGift in bannedGifts)
                {
                    if (gift.Name.ToLower().Equals(bannedGift.ToLower()))
                    {
                        giftbanned = true;
                        break;
                    }
                }

                if(giftbanned || alreadyOnList) { continue; }

                if(gift.Category == GiftCategory.WANT && wantCount < amount)
                {
                    child.Wishlist.Wanted.Add(gift);
                    wantCount++;
                }
                else if (gift.Category == GiftCategory.READ && readCount < amount)
                {
                    child.Wishlist.Wanted.Add(gift);
                    readCount++;
                }
                else if (gift.Category == GiftCategory.WEAR && wearCount < amount)
                {
                    child.Wishlist.Wanted.Add(gift);
                    wearCount++;
                }
                else if (gift.Category == GiftCategory.NEED && needCount < amount)
                {
                    child.Wishlist.Wanted.Add(gift);
                    needCount++;
                }

                if(wantCount == amount && readCount == amount && wearCount == amount && needCount == amount) { break; }
            }

            return child;
        }

        public static Child addOneGift(Child child)
        {
            IGiftRepository repo = new GiftRepository();
            List<Gift> gifts = repo.GetPossibleGifts();

            foreach (Gift gift in gifts)
            {
                if (child.Wishlist.Wanted.Contains(gift)) { continue; }
                child.Wishlist.Wanted.Add(gift);
                break;
            }

            return child;
        }

        public static Child addSpecificGift(Child child, string giftName)
        {
            IGiftRepository repo = new GiftRepository();
            List<Gift> gifts = repo.GetPossibleGifts();

            foreach (Gift gift in gifts)
            {
                if (child.Wishlist.Wanted.Contains(gift)) { continue; }
                if (gift.Name.ToLower().Equals(giftName.ToLower()))
                {
                    child.Wishlist.Wanted.Add(gift);
                    break;
                }
            }

            return child;
        }

        public static Child addCostumGift(Child child, string name, GiftCategory category)
        {
            Gift gift = new Gift();
            gift.Name = name;
            gift.Category = category;
            child.Wishlist.Wanted.Add(gift);
            
            return child;
        }

        public static bool searchError(string message, List<ValidationResult> errors)
        {
            foreach (ValidationResult result in errors)
            {
                if (result != ValidationResult.Success)
                {
                    if (result.ErrorMessage.Equals(message))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int countSuccesses(List<ValidationResult> errors)
        {
            int succescount = 0;
            foreach (ValidationResult result in errors)
            {
                if (result == ValidationResult.Success) { succescount++; }
            }
            return succescount;
        }
    }
}
