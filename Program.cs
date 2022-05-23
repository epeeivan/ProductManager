using System;
using Products;

namespace ProductManager
{
    public enum Etat
    {
        AVAILABLE,
        IN_BREAKDOWN,
        IN_SUPPLY
    }
    class Program
    {     
        private const int productNameMaxLenght = 44;
        private const int stockMaxSize = 10;
        private const int stateMaxSize = 24;
        static void Main(String[] args)
        {
            int n = 1;
            List<Product> produits = loadProductsFromFile("./products.txt");
            do{
                displayer(produits);

                // en attendte de saisie 
                var la = Console.ReadKey();
                
                int productIdToUpdateStock;
                bool temoin = int.TryParse(la.KeyChar.ToString(), out productIdToUpdateStock);
                if (temoin == true) {
                    // mise à jour du stock en ca s de consersion
                    foreach(var p in produits)
                    {
                        if(p.Id == productIdToUpdateStock)
                        {
                            p.Stock--;
                            if (p.Stock <= 0) p.Stock = 0;
                            p.Etat = exactState(p.Stock);
                        }
                    }
                    if (productIdToUpdateStock == 0)
                    {
                        Environment.Exit(0);
                    }
                } 
                Console.Clear();
            } while(n != 0);
        } 

        /*
        @name registerAvailableProductIds
        @param products
        @description retourne la liste des produits  
        */
        static List<int> registerAvailableProductIds(List<Product> products)
        {
            var ids = new List<int>();

            foreach (var item in products)
            {
                ids.Add(item.Id);
            }
            return ids;
        }

        /*
        @name loadProductsFromFile
        @param filePath
        @description rcharge tout les enregistrments du fichier de produits
        */      
        static List<Product> loadProductsFromFile(string filePath)
        {
            var lineProductsArray = File.ReadAllLines(filePath).ToList();
            var products = new List<Product>();

            int id;
            int stock;
            string name;
            Etat etat;
            string[] productDataArray;

            foreach( var line in lineProductsArray)
            {
                productDataArray = line.Split(":");

                int.TryParse(productDataArray[0], out id);
                name = productDataArray[1];

                int.TryParse(productDataArray[2], out stock);
                etat = exactState(stock);

                Product p = new Product(id, name, stock, etat);
                products.Add(p);
            }
            return products;
        }
        
        /*
        @name getStateString
        @param filePath
        @description retourne un état sous forme de chaine de caractère
        */    
        static string getStateString(Etat etat)
        {
            string state;

            switch (etat)
            {
                case Etat.AVAILABLE:
                    state = "Available";
                break;
                case Etat.IN_BREAKDOWN:
                    state = "Breakdown";
                break;
                case Etat.IN_SUPPLY:
                    state = "supply";
                break;
                default: state = "Breakdown";
                break;
            }
            return state;
        }

        /*
        @name exactState
        @param filePath
        @description retourne l"état tdu stock d'un produit
        */  
        static Etat exactState(int quantite)
        {
            Etat state = Etat.IN_BREAKDOWN;
            
            if (quantite > 5) state = Etat.AVAILABLE;
            if (quantite <= 5) state = Etat.IN_SUPPLY;
            if (quantite <= 0) state = Etat.IN_BREAKDOWN;
            
            return state;
        }

        /*
        @name displayer
        @param filePath
        @description elle permet l'displayer dans la console des différents éléments
        */  
        static void displayer(List<Product> produits)
        {
            Console.WriteLine("\n");
            Console.WriteLine("+---+--------------------------------------------+------------+-------------------------+");
            Console.WriteLine("| # | Produit(s)                                 |  Stock(s)  |           Etat          |");
            Console.WriteLine("+---+--------------------------------------------+------------+-------------------------+");

            foreach (var item in produits)
            {
                string id = "";
                if(item.Id<10) {
                    id ="| "+item.Id+" | ";
                }else {
                    id ="|"+item.Id+" | ";
                }
                Console.Write(id);  
                
                Console.Write(item.Name);
                spaceDisplayer(productNameMaxLenght - item.Name.Length - 1);
                Console.Write("| ");
                
                string qteStr = item.Stock.ToString();
                Console.Write(qteStr+" ");
                spaceDisplayer(stockMaxSize - qteStr.Length);
                Console.Write("| ");
                string etatStock = getStateString(item.Etat);
                Console.Write(etatStock);
                spaceDisplayer(stateMaxSize - etatStock.Length);
                Console.Write("|\n");
            }
            Console.WriteLine("+---+--------------------------------------------+------------+-------------------------+");
        }

        /*
        @name displayer
        @param filePath
        @description utilisée dans l'displayer des éléments 
        elle permet de placer des espaces un certain nombre de fois
        */  
        static void spaceDisplayer(int n)
        {
            for (int i = 0; i<n; i++) Console.Write(" ");
        }
	}
}