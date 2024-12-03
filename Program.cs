using Depara;

var listA = new List<string>() { "Muito a aprender você ainda tem" };
var listB = new List<string>() { "Você ainda tem muito a aprender", "Voces ainda tem muito a apreende" };

var possibilidades = new Dictionary<string, List<string>>();


foreach (var item in listA)
{
    foreach (var itemB in listB)
    {
        var calculadora = new CalculadorDistancia(item, itemB, 3);


        if (Math.Abs(calculadora.Possibility - 1.0) < 0.001)
        {
            Console.WriteLine($@"Match das palavras: {item} com a {itemB}");
            Console.WriteLine($@"{calculadora.ToString()}");
        }
        else if (calculadora.Possibility > 0.6)
        {
            if (!possibilidades.ContainsKey(item))
            {
                possibilidades.Add(item, new List<string>() { itemB });
                Console.WriteLine($@"{calculadora.ToString()}");
                // Outro tipo de Exemplo, porem no caso eu tinha a classe inteira pra trabalhar
                // Console.WriteLine($@"UPDATE TabelaB SET Descricao = '{item.Descricao}' where Id = '${itemB.Id}'");

            }
            else
            {
                if (possibilidades.TryGetValue(item, out List<string> lst))
                {
                    lst.Add(itemB);

                    Console.WriteLine($@"{calculadora.ToString()}");

                }
            }
        }
    }
}


Console.ReadLine();
