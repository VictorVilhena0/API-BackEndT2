using Charpter.WebApi.Controllers;
using Charpter.WebApi.Interfaces;
using Charpter.WebApi.Models;
using Charpter.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestXUnit.Controllers
{
    public class LoginControllerTest
    {
        [Fact]
        public void LoginController_DeveRetornar_UsuarioInvalido()
        {
            //Arrange
            var fakeRepository = new Mock<IUsuarioRepository>();
            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            LoginViewModel dadosLogin = new LoginViewModel();
            dadosLogin.Email = "backend@csharp.core";
            dadosLogin.Senha = "123abc";

            var controller = new LoginController(fakeRepository.Object);

            //Act
            var resultado = controller.Login(dadosLogin);

            //Assert
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        [Fact]
        public void LoginController_DeveRetornar_Token()
        {
            //Arrange
            Usuario usuarioRetorno = new Usuario();
            usuarioRetorno.Email = "backend@csharp.core";
            usuarioRetorno.Senha = "123abc";
            usuarioRetorno.Tipo = "11";

            var fakeRepository = new Mock<IUsuarioRepository>();
            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetorno);

            string issuerValidacao = "charpter.webapi";

            LoginViewModel dadosLogin = new LoginViewModel();
            dadosLogin.Email = "backend@csharp.core";
            dadosLogin.Senha = "123abc";

            var controller = new LoginController(fakeRepository.Object);

            //Act
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosLogin);

            string token = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenJwt = jwtHandler.ReadJwtToken(token);

            //Assert
            Assert.Equal(issuerValidacao, tokenJwt.Issuer);
        }

    }
}
