using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion_unidad_1
{
    public struct Vector2
    {
        public int x, y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 suma(Vector2 sumadoA)
        {
            int xsumada = x + sumadoA.x;
            int ysumada = y + sumadoA.y;
            Vector2 Solution = new Vector2(xsumada, ysumada);
            return Solution;
        }

        public override string ToString()
        {
            return ("(" + x.ToString() + " , " + y.ToString() + ")");
        }
    }
    public class Node
    {
        public bool explored = false;
        public Node exploredFrom;
        public Vector2 ubicacion;

        public Node(int x, int y)
        {
            ubicacion = new Vector2(x, y);
        }

        public Vector2 GetPosition()
        {
            return ubicacion;
        }

        public override string ToString()
        {
            return ("explorado: " + explored + "ubicacion: " + ubicacion);
        }
    }

    public class SearchPath
    {
        public SearchPath() { }
        //Inicio y Final del Camino
        private Node startPoint;
        private Node endPoint;

        //Direcciones posibles a recorrer
        private Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };

        //Se esta explorando y que esta explorando
        private bool isExploring = true;
        private Node searchingPoint;

        //Cola
        private Queue<Node> queue = new Queue<Node>();

        //Lista y Diccionarios
        private List<Node> path = new List<Node>();
        private Dictionary<Vector2, Node> Diccionario = new Dictionary<Vector2, Node>();
        private Dictionary<Vector2, int> Huecos = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, int> inifina = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, int> caminito = new Dictionary<Vector2, int>();

        //Tamaño del laberinto
        private int tamaño;

        //Valores iniciales del laberinto
        int xinicio = 2;
        int yinicio = 2;
        int xfinal = 4;
        int yfinal = 5;

        public void CreateMaze()
        {

            tamaño = 6;

            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    Vector2 vectorcreated = new Vector2(i, j);
                    Node nodecreated = new Node(i, j);
                    Diccionario.Add(vectorcreated, nodecreated);
                }
            }

         

            Vector2 inicio = new Vector2(xinicio, yinicio);
            Vector2 final = new Vector2(xfinal, yfinal);
            inifina.Add(inicio, 1);
            inifina.Add(final, 1);
            startPoint = Diccionario[inicio];
            endPoint = Diccionario[final];
            
            int cantidadDeHuecos = 2;

            for (int i = 1; i <= cantidadDeHuecos; i++)
            {
                int k = 2;
                int z = 3;
                int nuevok = 3;
                int nuevoz = 5;

                Vector2 hueco1 = new Vector2(k, z);
                Vector2 hueco2 = new Vector2(nuevok, nuevoz);

                if (Diccionario.ContainsKey(hueco1))
                {
                    Diccionario.Remove(hueco1);
                    Huecos.Add(hueco1, 1);
                }
                if (Diccionario.ContainsKey(hueco2))
                {
                    Diccionario.Remove(hueco2);
                    Huecos.Add(hueco2, 1);
                }
            }
            Console.WriteLine("Laberinto: ");
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    Vector2 dibujo = new Vector2(j, i);
                    if (inifina.ContainsKey(dibujo))
                    {
                        
                        Console.Write("H");
                        
                    }
                    else if (Diccionario.ContainsKey(dibujo))
                    {
                        Console.Write("O");
                    }
                    else if (Huecos.ContainsKey(dibujo))
                    {
                       
                        Console.Write("X");
                       
                    }
                }
                Console.WriteLine("");
            }


        }

        public void BFS()
        {
            queue.Enqueue(startPoint);
            while (queue.Count > 0 && isExploring)
            {
                searchingPoint = queue.Dequeue();
                OnReachingEnd();
                ExploreNeighbourNodes();
            }
        }

        public void OnReachingEnd()
        {
            if (searchingPoint == endPoint)
            {
                isExploring = false;
            }
            else
            {
                isExploring = true;
            }
        }

        public void ExploreNeighbourNodes()
        {
            if (!isExploring) { return; }
            foreach (var item in directions)
            {
                Vector2 vecinoPos = searchingPoint.GetPosition().suma(item);

                if (Diccionario.ContainsKey(vecinoPos))
                {
                    Node Nodovecino = Diccionario[vecinoPos];
                    if (!Nodovecino.explored)
                    {
                        Nodovecino.explored = true;
                        Nodovecino.exploredFrom = searchingPoint;
                        queue.Enqueue(Nodovecino);
                    }
                }

            }
        }

        public void CreatePath()
        {
            SetPath(endPoint);
            Node previousNode = endPoint.exploredFrom;

            while (previousNode != startPoint)
            {
                caminito.Add(previousNode.ubicacion, 1);
                SetPath(previousNode);
                previousNode = previousNode.exploredFrom;
            }

            SetPath(startPoint);
            path.Reverse();
        }

        public void SetPath(Node node)
        {
            path.Add(node);
        }

        public void ImprimirLista()
        {
            Console.WriteLine("Camino Recorrido: ");
            for (int i = 0; i < path.Count; i++)
            {
                Console.WriteLine(path[i].GetPosition());
            }
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    Vector2 dibujo = new Vector2(j, i);
                    if (inifina.ContainsKey(dibujo))
                    {
                       
                        Console.Write("H");
                      
                    }
                    else if (caminito.ContainsKey(dibujo))
                    {
                        
                        Console.Write("V");
                       
                    }
                    else if (Diccionario.ContainsKey(dibujo))
                    {
                        Console.Write("O");
                    }
                    else if (Huecos.ContainsKey(dibujo))
                    {
                        
                        Console.Write("X");
                        
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
