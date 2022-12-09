using Newtonsoft.Json;
using SantasWishList.Data.Models;
using SantasWishlist.Domain;

namespace SantasWishList.Logic;

public class ChildWishListBuilder
{
    private Child _child;

    public ChildWishListBuilder() => _child = new Child();
    
    public ChildWishListBuilder SetName(string name)
    {
        _child.Name = name;
        return this;
    }

    public ChildWishListBuilder SetIsNice(bool isNaughty)
    {
        _child.IsNice = isNaughty;
        return this;
    }

    public ChildWishListBuilder SetAge(int age)
    {
        _child.Age = age;
        return this;
    }

    public ChildWishListBuilder SetBehaviour(Behaviour behaviour)
    {
        _child.Behaviour = behaviour;
        return this;
    }

    public ChildWishListBuilder SetReasoning(string? reasoning)
    {
        _child.Reasoning = reasoning;
        return this;
    }

    public ChildWishListBuilder SetAdditionalGiftNames(List<string>? additionalGiftNames)
    {
        _child.AdditionalGiftNames = additionalGiftNames;
        return this;
    }

    public ChildWishListBuilder SetWishList(List<Gift> chosenGifts)
    {
        _child.Wishlist = new WishList { Name = _child.Name, Wanted = chosenGifts };
        return this;
    }
    
    public string Serialize() => JsonConvert.SerializeObject(_child);
    
    public ChildWishListBuilder Deserialize(string serializedChild)
    {
        _child = JsonConvert.DeserializeObject<Child>(serializedChild);
        return this;
    }

    public Child Build() => _child;
}