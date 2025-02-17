// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using Example;

var entity = new Entity()
{
    Name = "name",
    Age = 30
};

Expression<Func<Entity, bool>> expression1 = entry => entry.Name == "name";
Expression<Func<Entity, bool>> expression2 = entry => entry.Age > 10;
        
var expressionAnd = expression1.AndAlso(expression2);
        
var funcAnd = expressionAnd.Compile();

Console.WriteLine(funcAnd.Invoke(entity));