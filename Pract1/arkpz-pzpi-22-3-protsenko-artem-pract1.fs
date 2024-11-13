//Поганий приклад
let processData numbers = 
	let result = numbers |> List.filter (fun x -> x % 2 = 0) |> List.map (fun x -> x * 2) |> List.sum
	printfn "Result: %d" result

//Гарний приклад
let processData 
    (numbers: int list) = 
    numbers
    |> List.filter (fun x -> x % 2 = 0)
    |> List.map (fun x -> x * 2)
    |> List.sum                         
    |> printfn "Result: %d"

//Поганий приклад
module dataprocessingmodule

type customer_record = { customername: string; customerage: int }

type calculableInterface =
    abstract calcFunc: int -> int

let Calculate_TotalSum (itemslist: int list) = 
    itemslist |> List.sum

let calculate_customer_age_difference cust1 cust2 = 
    abs (cust1.customerage - cust2.customerage)

//Гарний приклад
module DataProcessing

type Customer = { Name: string; Age: int }

type ICalculable = 
    abstract Calculate: int -> int

let calculateTotal items = 
    items |> List.sum

let calculateAgeDifference customer1 customer2 = 
    abs (customer1.Age - customer2.Age)

//Поганий приклад
// Функція, яка отримує список чисел і повертає суму списку
let sumList lst = 
    lst |> List.sum

// Функція для обчислення середнього значення списку
let avgList lst = 
    let sum = lst |> List.sum // сумуємо список
    let len = List.length lst // рахуємо довжину
    sum / len // повертаємо середнє

//Гарний приклад
// Обчислює суму всіх чисел у списку.
// - parameter lst: Список чисел, який буде сумовано.
// - returns: Суму чисел у списку.
let sumList lst = 
    lst |> List.sum

// Обчислює середнє значення чисел у списку.
// - parameter lst: Список чисел.
// - returns: Середнє значення списку. Якщо список порожній, повертає 0.
let avgList lst = 
    match lst with
    | [] -> 0 
    | _ -> 
        let sum = lst |> List.sum
        let len = List.length lst
        sum / len

// Поганий приклад,
let mutable sum = 0
let addToSum x =
    sum <- sum + x

let result = [1; 2; 3; 4] |> List.map (fun x -> addToSum x) 

let computeData data =
    let result = List.fold (fun acc x -> acc + x) 0 data 
    result

//Гарний приклад
let addToSum x acc = acc + x

let result = [1; 2; 3; 4] |> List.fold addToSum 0 

let asyncComputeData data =
    async {
        // Створення списку асинхронних обчислень
        let! result = data |> List.map (fun x -> async { return x * 2 }) |> Async.Parallel
        return result |> Array.sum
    }
    
//Поганий приклад
let sumStrings (inputs: string list) =
    inputs
    |> List.map (fun s -> if s |> System.Int32.TryParse |> fst then int s else 0)
    |> List.sum

//Гарний приклад
let sumValidStrings (inputs: string list) : Result<int, string> =
    inputs
    |> List.map (fun s -> 
        match System.Int32.TryParse s with
        | (true, value) -> Ok value
        | (false, _) -> Error $"'{s}' is not a valid number")
    |> List.fold (fun acc elem ->
        match acc, elem with
        | Ok sum, Ok value -> Ok (sum + value)
        | Error msg, _ -> Error msg
        | _, Error msg -> Error msg) (Ok 0)

//Поганий приклад
let mutable list = []
for i in 1 .. 1000000 do
    list <- list @ [i]
let rec sumList lst =
    match lst with
    | [] -> 0
    | x::xs -> x + sumList xs
    
//Гарний приклад
let buildList n =
    let rec loop acc i =
        if i > n then acc
        else loop (i::acc) (i + 1) 
    loop [] 1 |> List.rev 

let sumList lst =
    let rec loop acc lst =
        match lst with
        | [] -> acc
        | x::xs -> loop (acc + x) xs 
    loop 0 lst