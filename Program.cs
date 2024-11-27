using Depara;

var listA = new List<Recheio>(){
    new Recheio(){Id = 1, Descricao = "abacate"},
    new Recheio(){Id = 2, Descricao = "morango"},
    new Recheio(){Id = 3,Descricao = "uva"},
    new Recheio(){Id = 3,Descricao = "abacaxi com coco"},
    };

var listB = new List<Fruta>(){
    new Fruta(){Id = 1, Descricao = "abacte"},
    new Fruta(){Id = 2,Descricao = "morango"},
    new Fruta(){Id = 3,Descricao = "uva"},
    new Fruta(){Id = 3,Descricao = "abacaxi"},
    new Fruta(){Id = 3,Descricao = "coco"},
    new Fruta(){Id = 3,Descricao = "hortela"},
};




foreach (var item in listA)
{
    foreach (var itemB in listB)
    {
        var calculadora = new CalculadorDistancia(item.Descricao, itemB.Descricao, 3);

        //Console.WriteLine($@"Comparação do Recheio: {item.Descricao} com a fruta {itemB.Descricao} - {calculadora.ToString()}");

        if (Math.Abs(calculadora.Possibility - 1.0) < 0.001)
        {
            item.Fruta = itemB;
        }
        else
        {
            if (calculadora.Possibility > 0.6)
                item.PossiveisFrutas.Add(itemB);
        }
    }
}

foreach (var item in listA)
{
    if (item.Fruta != null)
    {
        Console.WriteLine($@"Recheio {item.Descricao} - Possui combinação exata com {item.Fruta.Descricao}");

    }
    else
    {
        foreach (var possibilidade in item.PossiveisFrutas)
        {
            Console.WriteLine($@"Recheio {item.Descricao} - Possui possibilidade com {possibilidade.Descricao}");
        }
    }
}

Console.ReadLine();


public class Fruta
{
    public int Id { get; set; }
    public string Descricao { get; set; }
}

public class Recheio
{
    public int Id { get; set; }
    public string Descricao { get; set; }

    public Fruta Fruta { get; set; }

    public List<Fruta> PossiveisFrutas { get; set; } = new List<Fruta>();
}
