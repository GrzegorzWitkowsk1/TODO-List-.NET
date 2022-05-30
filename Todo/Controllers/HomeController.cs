using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Microsoft.Data.Sqlite;
using Todo.Models.ViewModels;
using Contact.Models;
using Privacy.Models;
using Privacy.Models.ViewModels;
namespace Todo.Controllers;

public class HomeController : Controller
{
   private readonly ILogger<HomeController> _logger;

   public HomeController(ILogger<HomeController> logger)
   {
      _logger = logger;
   }

   public IActionResult Index()
   {

      return View();
   }

   public IActionResult Privacy()
   {
      var privacyListViewModel = GetAllPrivacies();
      return View(privacyListViewModel);
   }

   public IActionResult Todo()
   {
      var todoListViewModel = GetAllTodos();
      return View(todoListViewModel);
   }

   public IActionResult Contact()
   {
      return View();
   }

   [HttpGet]

   public JsonResult PopulateForm(int id)
   {
      var todo = GetById(id);
      return Json(todo);
   }

   internal PrivacyViewModel GetAllPrivacies()
   {
      List<PrivacyItem> privacyList = new();

      using (SqliteConnection con =
             new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = con.CreateCommand())
         {
            con.Open();
            tableCmd.CommandText = "SELECT * FROM privacy";

            using (var reader = tableCmd.ExecuteReader())
            {
               if (reader.HasRows)
               {
                  while (reader.Read())
                  {
                     privacyList.Add(
                         new PrivacyItem
                         {
                            Id = reader.GetInt32(0),
                            PrivacyContent = reader.GetString(1),
                         });
                  }
               }
               else
               {
                  return new PrivacyViewModel
                  {
                     PrivacyList = privacyList
                  };
               }
            };
         }
      }

      return new PrivacyViewModel
      {
         PrivacyList = privacyList
      };
   }
   internal TodoViewModel GetAllTodos()
   {
      List<TodoItem> todoList = new();

      using (SqliteConnection con =
             new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = con.CreateCommand())
         {
            con.Open();
            tableCmd.CommandText = "SELECT * FROM todo";

            using (var reader = tableCmd.ExecuteReader())
            {
               if (reader.HasRows)
               {
                  while (reader.Read())
                  {
                     todoList.Add(
                         new TodoItem
                         {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Category = reader.GetString(2)
                         });
                  }
               }
               else
               {
                  return new TodoViewModel
                  {
                     TodoList = todoList
                  };
               }
            };
         }
      }

      return new TodoViewModel
      {
         TodoList = todoList
      };
   }

   public RedirectResult Insert(TodoItem todo)
   {
      using (SqliteConnection connection = new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = connection.CreateCommand())
         {
            connection.Open();
            tableCmd.CommandText = $"INSERT INTO todo (name,category) VALUES('{todo.Name}','{todo.Category}')";
            try
            {
               tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
         }
      }
      return Redirect("https://localhost:7161/Home/Todo");
   }

   public RedirectResult InsertContact(ContactItem contact)
   {
      using (SqliteConnection connection = new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = connection.CreateCommand())
         {
            connection.Open();
            tableCmd.CommandText = $"INSERT INTO contact (content) VALUES('{contact.Content}')";
            try
            {
               tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
         }
      }
      return Redirect("https://localhost:7161/Home/Contact");
   }


   [HttpPost]
   public JsonResult Delete(int id)
   {
      using (SqliteConnection connection =
             new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = connection.CreateCommand())
         {
            connection.Open();
            tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
            tableCmd.ExecuteNonQuery();
         }
      }

      return Json(new { });
   }

   internal TodoItem GetById(int id)
   {
      TodoItem todo = new();

      using (var connection =
      new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = connection.CreateCommand())
         {
            connection.Open();
            tableCmd.CommandText = $"SELECT * FROM todo where Id = '{id}'";

            using (var reader = tableCmd.ExecuteReader())
            {
               if (reader.HasRows)
               {
                  reader.Read();
                  todo.Id = reader.GetInt32(0);
                  todo.Name = reader.GetString(1);
                  todo.Category = reader.GetString(2);
               }
               else
               {
                  return todo;
               }
            };
         }
      }
      return todo;
   }
   public RedirectResult Update(TodoItem todo)
   {
      using (SqliteConnection con =
             new SqliteConnection("Data Source=db.sqlite"))
      {
         using (var tableCmd = con.CreateCommand())
         {
            con.Open();
            tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}', category = '{todo.Category}' WHERE Id = '{todo.Id}'";
            try
            {
               tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
         }
      }

      return Redirect("https://localhost:7161/Home/ToDo");
   }
}
