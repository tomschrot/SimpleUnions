
/*
    Simple example how to use Unions in a funtional way in C# 14. (New extension syntax!)
    (c) https://github.com/tomschrot
*/

WriteLine ("Unions in C# by tomschrot\n");

// Check for 2 cases:
WriteLine ("Check 2 with Error:");
for (int n = 0; n < 10; n++)
    RandomPet().Distinguish <Dog, Cat>
    (
        on1: HandleDog,
        on2: HandleCat,
        nil: HandleError // Error/Default action...
    );

// Check 3 cases; do nothing if outside range, short syntax
WriteLine ("\n\nCheck 3 no Error:");
for (int n = 0; n < 10; n++)
    RandomPet()
        .Distinguish <Dog, Cat, Bird>
        (
            HandleDog,
            HandleCat,
            HandleBird
        );

object RandomPet () =>
    Random.Shared.Next(4) switch
    {
        0 => new Dog  ("Rufus"),
        1 => new Cat  ("Felix"),
        2 => new Bird ("Lora"),
        _ => "Evil Plankton"
    };

void HandleDog  (Dog dog  ) => WriteLine ($"{dog.name} goes woof woof.");
void HandleCat  (Cat cat  ) => WriteLine ($"{cat.name} meows.");
void HandleBird (Bird bird) => WriteLine ($"{bird.name} goes chirp chirp.");

void HandleError (object obj) => WriteLine ($"I don't know {obj}");

WriteLine ($"\nOK! {DateTime.Now.ToString("HH:mm:ss")}");

// For Union by inheritance define any (virtual) type:
public abstract record Pet;

// Now declare any number of members:
public sealed record Dog  (string name): Pet;
public sealed record Cat  (string name): Pet;
public sealed record Bird (string name): Pet;

// To distinguish we use extensions.
// The usage is the same then e.g. Action<T>, Action <T1, T2>, Action<T1, T2, T3> etc.
public static class UnionExtension
{
    extension (object obj)
    {
        //---------------------------------------------------------------------
        // Handle only 2 cases:
        public object Distinguish <T1, T2>
        (
            Action<T1> on1 = null,
            Action<T2> on2 = null
            // Optional:
            , Action<object> nil = null
        )
        {
            switch (obj)
            {
                case T1 t1: on1?.Invoke (t1) ; break;
                case T2 t2: on2?.Invoke (t2) ; break;
                // Optional:
                default   : nil?.Invoke (obj); break;
            }

            return obj;
        }
        //---------------------------------------------------------------------
        // Handle 3 cases:
        public object Distinguish <T1, T2, T3>
        (
            Action<T1> on1 = null,
            Action<T2> on2 = null,
            Action<T3> on3 = null
            // Optional:
            , Action<object> nil = null
        )
        {
            switch (obj)
            {
                case T1 t1: on1?.Invoke (t1) ; break;
                case T2 t2: on2?.Invoke (t2) ; break;
                case T3 t3: on3?.Invoke (t3) ; break;
                // Optional:
                default   : nil?.Invoke (obj); break;
            }

            return obj;
        }
        //---------------------------------------------------------------------
        // Handle more cases...
        //---------------------------------------------------------------------
    }
}