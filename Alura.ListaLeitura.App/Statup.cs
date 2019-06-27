using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Alura.ListaLeitura.App
{
	public class Statup
	{
		public void Configure(IApplicationBuilder app)
		{
			/*IApplicationBuilder constroi o pipeline de requisição resposta para a aplicação*/
			/*Para qualquer requisição que chegar ela roda, pelo .run, o metodo Ler livros*/
			app.Run(Roteamento);
			
		}

		public Task Roteamento(HttpContext context)
		{
			/*Obtem o endereço que foi enviado na requisição*/

			//caminhos que serão atendidos pela aplicação
			// Livros/Paraler
			// Livros/Lendo
			// Livros/Lidos

			/*através da criação de um dicionário é possivel passar os caminhos esperados pela requisição
			 e para cada definição no Dictionary há uma resposta*/
			var _repo = new LivroRepositorioCSV();
			var caminhosAtendidos = new Dictionary<string, RequestDelegate>
			{
				{"/Livros/Paraler" , LivrosParaLer },
				{"/Livros/Lendo" , LivrosLendo },
				{"/Livros/Lidos" , LivrosLidos }
			};

			//verifica se o valor de request.path passado está contido no dicionário definido acima 
			if (caminhosAtendidos.ContainsKey(context.Request.Path))
			{
				var metodo = caminhosAtendidos[context.Request.Path];
				return metodo.Invoke(context);
			}

			//define que o código do retorno da requisição foi o 404, tendo resultado numa falha
			context.Response.StatusCode = 404;

			return context.Response.WriteAsync("Caminho inexistente");
		}

		public Task LivrosParaLer( HttpContext context)
		{
			/*Para que o metodo entre na pipeLine é necessário que ele retorne
			 uma resposta do tipo requestDelegate, e tem como retorno o tipo TASK, 
			 que por sua vez tem a ver com paralelismo*/
			 /**/
			var _repo = new LivroRepositorioCSV();
			return context.Response.WriteAsync(_repo.ParaLer.ToString());
		}

		public Task LivrosLendo(HttpContext context)
		{
			
			var _repo = new LivroRepositorioCSV();
			return context.Response.WriteAsync(_repo.Lendo.ToString());
		}

		public Task LivrosLidos(HttpContext context)
		{
			
			var _repo = new LivroRepositorioCSV();
			return context.Response.WriteAsync(_repo.Lidos.ToString());
		}
	}
}