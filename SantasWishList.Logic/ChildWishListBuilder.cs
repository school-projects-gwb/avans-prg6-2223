using SantasWishList.Data.Models;

namespace SantasWishList.Logic;

public class ChildWishListBuilder
{
    private Child _child;
    private bool _isBuilt;

    public ChildWishListBuilder() => _child = new Child();
    
    public void SetName(string name) => _child.Name = name;

    public void SetIsNaughty(bool isNaughty) => _child.IsNice = isNaughty;

    public void SetAge(int age) => _child.Age = age;

    public void SetBehaviour(Behaviour behaviour) => _child.Behaviour = behaviour;

    public void SetReasoning(string reasoning) => _child.Reasoning = reasoning;

    public void SetWishList()
    {
        throw new NotImplementedException();
    }

    public Child Build(string child)
    {
        //Parse json string
        //Run validation here
        _isBuilt = true;
        throw new NotImplementedException();
    }
    
    public string Stringify()
    {
        //JSON stringify object, if _isBuilt == true
        throw new NotImplementedException();
    }
}