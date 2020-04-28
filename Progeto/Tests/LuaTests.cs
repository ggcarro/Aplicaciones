using Microsoft.VisualStudio.TestTools.UnitTesting;
using Progeto.Lua;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class LuaTests
    {
        [TestMethod]
        public void LuaTest1()              // Nota: En lua las tildes funcionan (en el string normal, fallan)
        {
            string program = "print(\"Prueba realizada con exito!\")\n";    
            string result = "Prueba realizada con exito!\r\n";
            LuaInterpreter lua = new LuaInterpreter();
            string[] arrayTest = lua.RunProgram(program).ToArray();

            Assert.AreEqual(result, arrayTest[0]);
        }


        [TestMethod]
        public void LuaSvgTest1()
        {
            var svgStart = "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"drawing\" version=\"1.1\" height=\"100%\" width=\"100%\">\n";
            var resultSvg = "<circle cx=\"1\" cy=\"-2\" r=\"1\" style =\"stroke-width:1;stroke:rgb(1,2,3);fill:none;\"/>\n";
            var svgFinish = "</svg>";
            LuaInterpreter lua = new LuaInterpreter();
            var programSvg = lua.RunProgram("center = Point(1,-2)\nradius = 1\ntry = Circle(center,radius)\ncolor(1,2,3)\nwidth(1)\ndraw(try)");

            Assert.AreEqual(svgStart + resultSvg + svgFinish, programSvg.Cast<string>().ToArray()[1]);
        }


        [TestMethod]
        public void LuaSvgTest2()   //Vacío
        {
            string program = "print(\"Esto es una prueba\")";
            string svgStart = "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"drawing\" version=\"1.1\" height=\"100%\" width=\"100%\">\n";
            string svgFinish = "</svg>";
            LuaInterpreter lua = new LuaInterpreter();
            string[] arrayTest = lua.RunProgram(program).ToArray();

            Assert.AreEqual(svgStart + svgFinish, arrayTest[1]);
        }


        [TestMethod]
        public void LuaSvgTest3()
        {
            var svgStart = "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"drawing\" version=\"1.1\" height=\"100%\" width=\"100%\">\n";
            var resultSvg = "<circle cx=\"0\" cy=\"0\" r=\"-1\" style =\"stroke-width:1;stroke:rgb(1,0,1);fill:none;\"/>\n";
            var svgFinish = "</svg>";
            LuaInterpreter lua = new LuaInterpreter();
            var programSvg = lua.RunProgram("center = Point(0,0)\nradius = -1\ntry = Circle(center,radius)\ncolor(1,0,1)\nwidth(1)\ndraw(try)");

            Assert.AreEqual(svgStart + resultSvg + svgFinish, programSvg.Cast<string>().ToArray()[1]);
        }

    }
}
