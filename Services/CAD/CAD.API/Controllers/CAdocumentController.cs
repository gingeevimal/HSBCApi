using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Saml;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.WsFederation;
using System.Net.Http;
using RestSharp;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Ordering.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CAdocumentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private IWebHostEnvironment _hostEnvironment;


        public CAdocumentController(IMediator mediator, IWebHostEnvironment environment)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _hostEnvironment = environment;

        }

        //[HttpGet("{userName}", Name = "GetCAdocument")]
        [HttpGet]
        [Route("GetCAdocument")]
        [ProducesResponseType(typeof(IEnumerable<CAdocumentVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CAdocumentVm>>> GetCAdocumentByUserName(string businesspartner)
        {
            var query = new GetCAdocumentListQuery(businesspartner);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        // testing purpose
        // [HttpPost(Name = "InsertCAdocument")]
        [HttpPost]
        [Route("InsertCAdocument")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> InsertCAdocument([FromBody] InsertCAdocumentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //[HttpPut(Name = "UpdateCAdocument")]
        [HttpPut]
        [Route("UpdateCAdocument")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateCAdocument([FromBody] UpdateCAdocumentCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        //[HttpDelete("{id}", Name = "DeleteCAdocument")]
        [HttpDelete]
        [Route("DeleteCAdocument")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCAdocument(int id)
        {
            var command = new DeleteCAdocumentCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult Login()
        {
           // SamlConsume();
            //TODO: specify the SAML provider url here, aka "Endpoint"
            //var samlEndpoint = "http://saml-provider-that-we-use.com/login/";
            var samlEndpoint = "https://login.microsoftonline.com/b9165cc7-82cb-4b2f-86eb-f60b7e3809a5/saml2";

            var request = new AuthRequest(
                "https://hoekstra.dev/SAMLBlogPost", //TODO: put your app's "entity ID" here
                "https://localhost:44373/api/v1/CAdocument/SamlConsume" //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
                //"http://localhost:4200/" 
                );
            //var request = new AuthRequest(
            //"http://www.myapp.com", //TODO: put your app's "entity ID" here
            //"http://www.myapp.com/SamlConsume" //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
            //);
            var redirectUrl = request.GetRedirectUrl(samlEndpoint);

            HttpClient client = new HttpClient();

            // Send a request asynchronously continue when complete 
            client.GetAsync(redirectUrl).ContinueWith(
                (requestTask) =>
                {
        // Get HTTP response from completed task. 
        HttpResponseMessage response = requestTask.Result;

        // Check that response was successful or throw exception 
        response.EnsureSuccessStatusCode();

        // Read response asynchronously as JsonValue
        //response.Content.ReadAsAsync<JsonArray>().ContinueWith(
        //                        (readTask) =>
        //                        {
        //                            var result = readTask.Result
        //                            //Do something with the result                   
        //            });
                });
            //redirect the user to the SAML provider
            return Redirect(request.GetRedirectUrl(redirectUrl));
            //return Challenge(
            //    new AuthenticationProperties { RedirectUri = redirectUrl },
            //    WsFederationDefaults.AuthenticationScheme);
        }
        [HttpPost]
        [Route("SamlConsume")]
        public object SamlConsume()
        {

             //https://github.com/jitbit/AspNetSaml    POC Link

            // 1. TODO: specify the certificate that your SAML provider gave you

            string path = Path.Combine(_hostEnvironment.ContentRootPath, "SAMLBlogpost.xml");

            XmlDocument _LocalInfo_Xml = new XmlDocument();
            _LocalInfo_Xml.Load(path);
            XmlElement _XmlElement;
            _XmlElement = _LocalInfo_Xml.GetElementsByTagName("X509Certificate")[0] as XmlElement;
            string samlCertificate = _XmlElement.InnerText;

            // 2. Let's read the data - SAML providers usually POST it into the "SAMLResponse" var
            //Saml.Response samlResponse = new Response(samlCertificate, Request.Form["SAMLResponse"]);
            string Samlresponse = Request.Form["Samlresponse"];
            Response samlResponse = new Response(samlCertificate, Samlresponse);

            // 3. We're done!
            if (samlResponse.IsValid())
            {
                //WOOHOO!!! user is logged in

                //Some more optional stuff for you
                //let's extract username/firstname etc
                string username, email, firstname, lastname;
                try
                {
                    username = samlResponse.GetNameID();
                    email = samlResponse.GetEmail();
                    firstname = samlResponse.GetFirstName();
                    lastname = samlResponse.GetLastName();
                    return Ok(username);
                }
                
                catch (Exception ex)
                {
                    //insert error handling code
                    //no, really, please do
                    return null;
                }

                //user has been authenticated, put your code here, like set a cookie or something...
                //or call FormsAuthentication.SetAuthCookie() or something
            }
            return Ok(string.Empty);
        }
    }
}
