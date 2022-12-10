namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        void Realizartarea();
    }
    public class ServicioA : IServicio
    {
        private readonly ILogger<ServicioA> logger;

        public ServicioA(ILogger<ServicioA> logger)
        {
                this.logger = logger;
        }

        public void Realizartarea()
        {

        }

    }
    public class ServicioB : IServicio
    {

        public void Realizartarea()
        {

        }

    }
}
