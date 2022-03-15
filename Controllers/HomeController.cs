using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WANTED.Deserializar;
using System.Text;
using WANTED.Models;
using System.Net;
using System.IO;

namespace WANTED.Controllers
{
    public class HomeController : Controller
    {
    private byte[] content = null;
        public async Task<ActionResult> Index()
        {
            bool respuesta =  await SaveTodisk("col");


            List<Intl_Buscados> buscar = new List<Intl_Buscados>();
            using (CONEXION db = new CONEXION())
            {
             buscar=   db.Intl_Buscados.ToList();

            }
             


            return View(buscar);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        

        public async Task<dynamic> Prueba(int cantidad)
        {   string paginabase = "https://api.fbi.gov";
            string Categoria = "/wanted/v1/list";
            string pagina = "page=";
            string numero=cantidad.ToString();
            Intl_Buscados buscado = new Intl_Buscados();
            //string pagina= https://api.fbi.gov/wanted/v1/list?page=2;
            string url = "https://api.fbi.gov/wanted/v1/list";
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri(paginabase);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            


            var  response = await cliente.GetAsync("https://api.fbi.gov/wanted/v1/list?page="+numero);
            if(response.IsSuccessStatusCode)
            {

                var json= await response.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<dynamic>(json);
                var Data = datos["items"];
                Rasgos objeto = new Rasgos();
                List <string> Campos=new List<string>();
                foreach (var item in Data)

                {

                

                    using (CONEXION db = new CONEXION())
                    {
                        try
                        {
                            if(item.title.Value==null)
                            {
                                objeto.Nombre = "nulo";
                            }
                            else
                            {
                                objeto.Nombre= item.title.Value;
                            }

                            if(item.images[0].large.Value==null)
                            {
                                objeto.images = "nulo";
                            }
                            else
                            {
                                objeto.images= item.images[0].large.Value;
                            }
                            if(item.sex.ToString()==null)
                            {
                                objeto.sex = "nulo";
                            }
                            else
                            {
                                objeto.sex = item.sex.ToString();
                            }


                            if(item.subjects[0].ToString()==null)
                            {
                                objeto.Categoria = "nulo";
                            }
                            else
                            {
                                objeto.Categoria = item.subjects[0].ToString(); 
                            }

                            if(item.nationality.ToString()==null)
                            {
                                objeto.nacionalidad = "nulo";
                            }
                            else
                            {
                                objeto.nacionalidad = item.nationality.ToString();
                            }
                            if(item.age_min.ToString()==null)
                            {

                                objeto.EdadMinima = "nulo";
                            }
                            else
                            {
                                objeto.EdadMinima = item.age_min.ToString();
                            }


                                if(item.place_of_birth.ToString()==null)
                                {
                               
                                    objeto.LugarNacimiento = "nulo";
                                }
                                else
                                {
                                objeto.LugarNacimiento = item.place_of_birth.ToString();

                                }
                            if (item.age_range.ToString() == null)
                            {
                               
                                objeto.Rango_de_Edad = "nulo";
                            }
                            else
                            {
                            
                            objeto.Rango_de_Edad = item.age_range.ToString();

                            }

                           
                            //if(item.dates_of_birth_used[0].Value==null)
                            //{
                            //    objeto.FechaNacimiento = "nulo";

                            //}
                            //else
                            //{

                            //objeto.FechaNacimiento = item.dates_of_birth_used.ToString();
                            //    if(item.dates_of_birth_used[0].ToString()!=null)
                            //    {
                            //        objeto.FechaNacimiento = item.dates_of_birth_used[0].ToString();
                            //    }
                            //}

                            if(item.remarks.Value==null)
                            {
                                objeto.Observaciones = "nulo";
                            }
                            else
                            {
                                objeto.Observaciones= item.remarks.Value.ToString();  
                            }



                            if(item.details.Value==null)
                            {
                                objeto.Detalle = "nulo";
                            }
                            else
                            {
                                objeto.Detalle= item.details.Value.ToString();  
                            }

                            //if (item.sex.Value == null)
                            //{

                            //    objeto.Nombre = item.title.Value;                      
                            //    objeto.images = item.images[0].large.Value;
                            //    objeto.sex = "nulo";
                            //    objeto.Categoria = item.subjects[0].ToString();
                            //}
                            //else
                            //{
                            //    objeto.Nombre = item.title.Value;
                            //    objeto.images = item.images[0].large.Value;
                            //    objeto.sex = item.sex.ToString();
                            //    objeto.Categoria = item.subjects[0].ToString();
                            //}



                            






                            buscado.nombre = objeto.Nombre;                         
                            buscado.Link_Imagen = objeto.images.ToString();
                            buscado.Genero = objeto.sex.ToString();
                            buscado.Nacionalidad = objeto.nacionalidad.ToString();
                            buscado.Edad_Minima = objeto.EdadMinima.ToString();
                            buscado.Categoria = objeto.Categoria.ToString();
                            buscado.Lugar_Nacimiento = objeto.LugarNacimiento.ToString();
                            buscado.Rango_Edad = objeto.Rango_de_Edad.ToString();
                            buscado.Fecha_Nacimiento = objeto.FechaNacimiento.ToString();
                            buscado.Observaciones = objeto.Observaciones.ToString(); 
                        }
                        catch (Exception ex)
                        {
                            string mensaje = ex.ToString();
                         

                        }
                        var persona = db.Intl_Buscados.Where(a => a.nombre == buscado.nombre).FirstOrDefault();
                        if(persona==null)
                        {
                            db.Intl_Buscados.Add(buscado);
                            db.SaveChanges();


                        }
                        else
                        {
                            continue;
                        }


                    }

                }
                return response;
                //data['items'][0]['title']
            }
            else
            {
                return null;
            }


            //WebClient myWebClient = new WebClient();
            //ID1 = cedula.Substring(0, 3);

            //ID2 = cedula.Substring(3, 7);
            //ID3 = cedula.Substring(10, 1);

            //string portal = "https://dataportal.jce.gob.do";
            //string AUrl = "https://dataportal.jce.gob.do/idcons/IndividualDataHandler.aspx?ServiceID="
            //    + ServiceID + "&ID1=" + ID1 + "&ID2=" + ID2 + "&ID3=" + ID3;
            //HttpClient httpclient = new HttpClient();
            //WebClient webClient = new WebClient();
            //webClient.Headers.Add("user-agent", "Consumiendo Servicio C5i");
            //System.Threading.Thread.Sleep(ramd);
            //var res = webClient.DownloadString(AUrl);

            //var xmlDocument = new XmlDocument();












        }

        public async Task<ActionResult> Buscados()
        {
            bool respuesta = await SaveTodisk("br");

            List<Intl_Buscados> buscar = new List<Intl_Buscados>();
            using (CONEXION db = new CONEXION())
            {
                buscar = db.Intl_Buscados.ToList();

            }



            return View(buscar);
        }

        public ActionResult BuscadosInterPol()
        {
            List<Interpol> interpol = new List<Interpol>();

           using (CONEXION db = new CONEXION()) 
            {
                    interpol=  db.Interpol.ToList();            
            
            }

            return View(interpol);
            

           
        }



        public async Task<bool> SaveTodisk(string codigoPais="br")
        {
            bool mensaje=false;
            string Nacionalidad = string.Empty;
            Nacionalidad = codigoPais;
            int EdadMaxima = 120, EdadMinima = 18;

            string Fuente = "https://ws-public.interpol.int/notices/v1/red?nationality=" + Nacionalidad + "&ageMax=" + EdadMaxima + "&ageMin=" + EdadMinima;


            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri(Fuente);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await cliente.GetAsync(Fuente);
            Interpol policia = new Interpol();
            if (response.IsSuccessStatusCode)
            {

                var json1 = await response.Content.ReadAsStringAsync();
                var datos1 = JsonConvert.DeserializeObject<dynamic>(json1);

                int contador = 0;
                foreach (var item in datos1._embedded.notices)
                {
                    contador++;
                    if (item.nationalities.Count > 1)
                    {
                        policia.Nacionalidad2 = item.nationalities[1];

                    }
                    policia.entity_id = item.entity_id;
                    policia.Fecha_Nacimiento = item.date_of_birth.ToString();
                    policia.Nombre_Pila = item.forename;
                    policia.Nacionalidad = item.nationalities[0];
                    policia.LinkServicio = item._links.self.href.ToString();

                    var respuesta = await cliente.GetAsync(policia.LinkServicio);
                    string UrlImagen = string.Empty;
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var json = await respuesta.Content.ReadAsStringAsync();
                        var datos = JsonConvert.DeserializeObject<dynamic>(json);
                        policia.Peso_Corporal = datos.weight;



                        if (contador ==7)
                        {
                            string detente = string.Empty;
                        }



                        if (datos.languages_spoken_ids!= null)
                        {
                            var len = datos.languages_spoken_ids[0].Value.ToString();
                            policia.Idioma_Hablado = len;
                        }
                        int toatal = contador;
                     
                        policia.Pais_Incidente = datos.arrest_warrants[0].issuing_country_id;
                        policia.Altura = datos.height;
                        policia.Genero = datos.sex_id;
                        policia.Pais_Nacimiento = datos.country_of_birth_id;
                        policia.Nombre = datos.name;
                        if(datos.distinguishing_marks.Value!=null)
                        {
                        policia.Marcas_Distintivas = datos.distinguishing_marks;

                        }
                        if(datos.eyes_colors_id!=null)
                        {
                            policia.Codigo_Color_Ojos =Convert.ToString(datos.eyes_colors_id);
                        }
                       if(datos.hairs_id!=null)
                        {
                            policia.Codigo_Color_Cabello = Convert.ToString(datos.hairs_id);

                        }
                      
                        policia.Lugar_Nacimiento = datos.place_of_birth;
                        policia.Cargos = datos.arrest_warrants[0].charge;
                        policia.Interpretacion_Cargos = datos.arrest_warrants[0].charge_translation;
                        UrlImagen = datos._links.images.href;
                    }


                    var RespuestaImg = await cliente.GetAsync(UrlImagen);
                    if (RespuestaImg.IsSuccessStatusCode)
                    {
                       
                        var jsonImg = await RespuestaImg.Content.ReadAsStringAsync();
                        var datosImg = JsonConvert.DeserializeObject<dynamic>(jsonImg);
                        if(datosImg!=null)
                        {
                            
                            if(datosImg._embedded.images.Count>0)
                            {
                                policia.LinkImagen = datosImg._embedded.images[0]._links.self.href.Value;
                            }
                            else if(datosImg._links.self.href!=null)
                            {
                                policia.LinkImagen = datosImg._links.self.href.Value;
                            }
                            else
                            {

                            }
                           
                        }
                        
                      
                    }



                    //Mapeando Datos



                    using (CONEXION db = new CONEXION())
                    {

                        var persona = db.Interpol.Where(a => a.Nombre == policia.Nombre || a.Nombre_Pila == policia.Nombre_Pila).FirstOrDefault();

                        if (persona == null)
                        {
                            db.Interpol.Add(policia);
                            db.SaveChanges();
                            mensaje = true;
                            WebClient webClient = new WebClient();
                            webClient.Headers.Add("user-agent", "Consumiendo Servicio");
                            content = webClient.DownloadData(policia.LinkImagen);
                            SaveImg(content,policia.Id);
                        }

                    }

                }

                





            }


            return mensaje;
        }















        public ActionResult Interpol()
        {



            return View();
        }
      


       private async Task<string > SearchInterpol()
        {
            //ListInterPoll datos= new InterPoll();
            List<string> texto = new List<string>();
            string Nacionalidad=string.Empty;
            Nacionalidad = "do";
            int EdadMaxima = 120, EdadMinima=18;

            string Fuente = "https://ws-public.interpol.int/notices/v1/red?nationality=" + Nacionalidad + "&ageMax=" + EdadMaxima + "&ageMin=" + EdadMinima;
                      
            
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri(Fuente);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await cliente.GetAsync(Fuente);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
              var   datos = JsonConvert.DeserializeObject<List<InterPoll>>(json);
                Session["Datos"] = datos;
                Rasgos objeto = new Rasgos();
                List<string> Campos = new List<string>();
            }


                return "";
        }



        private ActionResult SaveImg(byte[] foto,int Id)
        {
            

            return null;
        }

        public static async Task DownloadAsync(Uri requestUri, string filename)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (
                Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
            {
                await contentStream.CopyToAsync(stream);
            }
        }
    }
}