namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public List<Libro> Libros { get; set; }
    }
}
