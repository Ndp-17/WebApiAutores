namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSingelton();
        Guid ObtenerTransient();
        void Realizartarea();
    }
    public class ServicioA : IServicio
    {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingelton servicioSingelton;

        public ServicioA(ILogger<ServicioA> logger
            , ServicioTransient servicioTransient
            , ServicioScoped servicioScoped
            , ServicioSingelton servicioSingelton)
        {
                this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingelton = servicioSingelton;
        }

        public Guid ObtenerTransient() { return servicioTransient.Guid; }
        public Guid ObtenerScoped() { return servicioScoped.Guid; }
        public Guid ObtenerSingelton() { return servicioSingelton.Guid; }

        public void Realizartarea()
        {

        }

    }
    public class ServicioB : IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingelton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void Realizartarea()
        {

        }

    }

    public class ServicioTransient 
    { 
        public Guid Guid = Guid.NewGuid();
    }
    public class ServicioScoped
    { 
        public Guid Guid = Guid.NewGuid();
    }
    public class ServicioSingelton
    { 
        public Guid Guid = Guid.NewGuid();
    }
}
