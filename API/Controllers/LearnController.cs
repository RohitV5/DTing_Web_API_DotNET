using System.Collections;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

//Make this class private and this API is not accessible on swagger.
public class LearnController : BaseApiController
{

    //Generally we are using constuctor to inject dependecy.
    //Since this controller has been created to learn C# concepts we will leave it blank.
    //We could have removed it but just keeping it for sake of learning
    public LearnController()
    {
        //constructor method is same as class name.
        //anything passed in constrcutor method should be avalaible during object instantiation.
    }


    /* DATATYPES */



    [HttpGet("datatypes")]
    public ActionResult<string> Datatypes()
    // public int[] GetArrays ()  // this is also valid
    {
        //ARRAY - fast same type element access in contigious memory location
        int[] nums = { 1, 2, 3, 4, 5 };
        // now you can perform built in array operations on this variable like sum, average etc.

        // LIST - Resizable collection wih add/remove methods
        var list = new List<string> { "Saka", "Martinelli", "Xhaka" };

        //DICTIONARIES = Key value collection with add remove access by keys
        var countryCode = new Dictionary<string, string>();

        countryCode.Add("IN", "India");
        countryCode.Add("SW", "Sweden");
        countryCode.Add("Key", "Value");

        //Hash table - Uses hash function to map keys to array indexes for fast access
        //Key value
        var location = new Hashtable();  // imported from system.collections 
        location.Add("L", "London");
        location.Add("M", "Madrid");
        location.Add("Key", "Value");


        //HashSet - Unique element collections with add/remove/check method
        var discounts = new HashSet<int> { 7, 5, 6 };


        // Linear data structure, nodes have data and reference to next node
        var linkedlist = new LinkedList<string>();
        linkedlist.AddLast("hello");
        linkedlist.AddLast("world");


        // FIFO structure for adding/removing elements from the top
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);


        //LIFO structure for removing the oldest element first
        var queue = new Queue<int>{};
        queue.Enqueue(1);
        queue.Enqueue(2);


        // () Can be be replaced by {} in some places
        // new Stack<int>(); === new Stack<int>{};














        return JsonSerializer.Serialize(new
        {
            arrays = nums,
            list, //shorthand
            dictionary = countryCode,
            hashtable = location,
            hashSet = discounts,
            linkedlist, //shorthand
            stack, //shorthand
            queue,
        });


    }





    /*LAMBDA FUNCTION*/
    [HttpGet("getlambda/{number}")]
    public ActionResult<int> getLambda(int number)
    {
        // Here x => x * x is the lambda.
        //In software world anonymous functions are called lambda
        Func<int, int> square = x => x * x;

        return square(number);

        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
    }






}

