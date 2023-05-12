using BlazorFullStackCrud.Client.Pages;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorFullStackCrud.Client.Services.SuperHeroService;

public class SuperHeroService : ISuperHeroService
{
    private readonly HttpClient http;
    private readonly NavigationManager navigationManager;

    public List<SuperHero> Heroes { get; set; } = new();
    public List<Comic> Comics { get; set; } = new();

    public SuperHeroService(HttpClient http, NavigationManager navigationManager)
    {
        this.http = http;
        this.navigationManager = navigationManager;
    }

    public async Task GetComics()
    {
        var result = await http.GetFromJsonAsync<List<Comic>>("api/superhero/comics");
        if (result != null)
        {
            Comics = result;
        }
    }

    public async Task<SuperHero> GetSingleHero(int id)
    {
        var result = await http.GetFromJsonAsync<SuperHero>($"api/superhero/{id}");
        if (result != null)
        {
            return result;
        }
        throw new Exception("Hero not found!");
    }

    public async Task GetSuperHeroes()
    {
        var result = await http.GetFromJsonAsync<List<SuperHero>>("api/superhero");
        if(result is not null)
        {
            Heroes = result;
        }
    }

    public async Task CreateSuperHero(SuperHero hero)
    {
        var result = await http.PostAsJsonAsync("api/superhero", hero);
        await SetHeroes(result);
    }

    private async Task SetHeroes(HttpResponseMessage result)
    {
        var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
        if(response is not null)
            Heroes = response;

        navigationManager.NavigateTo("/");
    }

    public async Task UpdateHero(SuperHero hero)
    {
        var result = await http.PutAsJsonAsync($"api/superhero/{hero.Id}", hero);
        await SetHeroes(result);
    }

    public async Task DeleteHero(int id)
    {
        var result = await http.DeleteAsync($"api/superhero/{id}");
        await SetHeroes(result);
    }
}
