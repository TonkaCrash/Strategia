﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;

namespace Strategia
{
    public static class CelestialBodyUtil
    {
        public static IEnumerable<CelestialBody> GetBodiesForStrategy(string id)
        {
            CelestialBody home = FlightGlobals.Bodies.Where(cb => cb.isHomeWorld).Single();

            if (id == "MoonProgram")
            {
                foreach (CelestialBody child in home.orbitingBodies)
                {
                    yield return child;
                }

                // Special case for mods where Kerbin is a Gas Giant's moon
                if (home.referenceBody != FlightGlobals.Bodies[0])
                {
                    foreach (CelestialBody child in home.referenceBody.orbitingBodies.Where(cb => cb != home))
                    {
                        yield return child;
                    }
                }
            }
            else if (id == "PlanetaryProgram")
            {
                foreach (CelestialBody body in FlightGlobals.Bodies[0].orbitingBodies)
                {
                    if (body != home && body.pqsController != null)
                    {
                        yield return body;
                    }
                }
            }
            else if (id == "GasGiantProgram")
            {
                foreach (CelestialBody body in FlightGlobals.Bodies[0].orbitingBodies)
                {
                    if (body.pqsController == null && !body.orbitingBodies.Contains(home) && body.orbitingBodies.Count() >= 2)
                    {
                        yield return body;
                    }
                }
            }
            else
            {
                foreach (CelestialBody body in FlightGlobals.Bodies)
                {
                    yield return body;
                }
            }
        }

        public static string BodyList(IEnumerable<CelestialBody> bodies, string conjunction)
        {
            CelestialBody first = bodies.First();
            CelestialBody last = bodies.Last();
            string result = first.theName;
            foreach (CelestialBody body in bodies.Where(cb => cb != first && cb != last))
            {
                result += ", " + body.theName;
            }
            if (last != first)
            {
                result += " " + conjunction + " " + last.theName;
            }
            return result;
        }

    }
}