using SantasWishList.Data.Models;

namespace SantasWishList.Logic;

public class ChildWishListBuilder
{
    private Child _child;
    private bool _isBuilt;

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

    public ChildWishListBuilder SetReasoning(string reasoning)
    {
        _child.Reasoning = reasoning;
        return this;
    }

    public ChildWishListBuilder SetWishList()
    {
        return this;
    }

    public Child Build()
    {
        //Parse json string
        //Run validation here
        _isBuilt = true;
        return _child;
    }
    
    public string Stringify()
    {
        //JSON stringify object, if _isBuilt == true
        throw new NotImplementedException();
    }
}