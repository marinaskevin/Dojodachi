using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojodachi
{
    public class DojodachiController : Controller
    {
        Random rand = new Random();

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("Meals") == null)
            {
                HttpContext.Session.SetInt32("Happiness",20);
                HttpContext.Session.SetInt32("Fullness",20);
                HttpContext.Session.SetInt32("Energy",50);                
                HttpContext.Session.SetInt32("Meals",3);
                HttpContext.Session.SetString("Message","");
                HttpContext.Session.SetString("Graphic","img/marching_pikachus.gif");
            }
            ViewBag.Happiness = (int)HttpContext.Session.GetInt32("Happiness");
            ViewBag.Fullness = (int)HttpContext.Session.GetInt32("Fullness");
            ViewBag.Energy = (int)HttpContext.Session.GetInt32("Energy");
            ViewBag.Meals = (int)HttpContext.Session.GetInt32("Meals");
            ViewBag.Message = HttpContext.Session.GetString("Message");
            ViewBag.Graphic = HttpContext.Session.GetString("Graphic");
            return View();
        }

        [HttpGet]
        [Route("Restart")]
        public IActionResult Restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Interact/{dothis}")]
        public object Interact(string dothis)
        {
            int happiness = (int)HttpContext.Session.GetInt32("Happiness");
            int fullness = (int)HttpContext.Session.GetInt32("Fullness");
            int energy = (int)HttpContext.Session.GetInt32("Energy");
            int meals = (int)HttpContext.Session.GetInt32("Meals");
            string message = "";
            string graphic = "";
            bool win = false;
            bool lose = false;
            if(dothis == "Feed")
            {
                if(meals > 0)
                {
                    meals--;
                    int reaction = rand.Next(0,4);
                    Console.WriteLine(reaction);
                    if(reaction == 0)
                    {
                        message = "Dojodachi no want it!";
                        graphic = "not_hungry.png";
                    }
                    else
                    {
                        fullness += rand.Next(5,11);
                        if(fullness > 60)
                        {
                            message = "Dojodachi is filling up well. Keep at it!";
                            graphic = "pikachu_getting_full.gif";
                        }
                        else
                        {
                            message = "Nom. Nom. Nom. More please!";
                            graphic = "yummy.gif";
                        }
                    }
                }
                else
                {
                    message = "No more meals!";
                    graphic = "no_more_meals.gif";
                }
            }
            if(dothis == "Play")
            {
                int reaction = rand.Next(0,4);
                energy -= 5;
                if(reaction > 0)
                {
                    happiness += rand.Next(5,11);
                    if(happiness > 60 && energy > 60)
                    {
                        message = "Dojodachi is excited to play!";
                        graphic = "excited_to_play.gif";
                    }
                    else
                    {
                        message = "Dojodachi is ready to play!";
                        graphic = "ready_to_play.gif";
                    }
                }
                else
                {
                    if(energy < 30)
                    {
                        message = "Dojodachi sleep instead!";
                        graphic = "exhausted.gif";
                    }
                    else if(happiness < 30)
                    {
                        message = "Dojodachi is sad and doesn't want to play!";
                        graphic = "sadness.gif";
                    }
                    else if(meals < 2)
                    {
                        message = "Dojodachi has no time for fun and games. Need more meals!";
                        graphic = "gotta_run.gif";
                    }
                    else{
                        message = "Pikachu is hiding!";
                        graphic = "no_play.png";
                    }
                }
                if(energy < 0)
                {
                    message = "Dojodachi is exhausted is now sleeping forever.";
                    graphic = "sleeping_forever.jpg";
                    lose = true;
                }
            }
            if(dothis == "Work")
            {
                energy -= 5;
                int more = rand.Next(1,4);
                if(energy < 0)
                {
                    message = "Dojodachi is exhausted is now sleeping forever. ";
                    graphic = "sleeping_forever.jpg";
                    lose = true;
                }
                else
                {
                    meals += more;
                    if(more > 1)
                    {
                        message = "Let's get this done!";
                        graphic = "important.gif";
                    }
                    else
                    {
                        message = "Let's take care of this first!";
                        graphic = "gotta_run.gif";
                    }
                }
                
            }
            if(dothis == "Sleep")
            {
                energy += 15;
                fullness -= 5;
                happiness -= 5;
                message = "Sleepy time!";
                graphic = "exhausted.gif";
                if(fullness <= 0)
                {
                    message = "Dojodachi has died of hunger.";
                    graphic = "last_rest.png";
                    lose = true;
                }
            }
            if(energy > 100 && fullness > 100 && happiness > 100)
            {
                message = "Dojodachi now lives forever as an invicible being. You Win!";
                graphic = "pikachu_invincible.png";
                win = true;
            }
            HttpContext.Session.SetInt32("Happiness",happiness);
            HttpContext.Session.SetInt32("Fullness",fullness);
            HttpContext.Session.SetInt32("Energy",energy);
            HttpContext.Session.SetInt32("Meals",meals);
            HttpContext.Session.SetString("Message",message);
            HttpContext.Session.SetString("Graphic",graphic);
            object status = new {
                happiness = happiness,
                fullness = fullness,
                energy = energy,
                meals = meals,
                message = message,
                graphic = graphic,
                win = win,
                lose = lose
            };
            return status;
        }
    }
}