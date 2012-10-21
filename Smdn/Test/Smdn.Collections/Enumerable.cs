using System;
using System.Collections;
using System.Collections.Generic;
#if NET_3_5
using System.Linq;
#endif
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture()]
  public class EnumerableTests {
    class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Vaccinated { get; set; }
    }

    [Test]
    public void TestSequenceEqual()
    {
      Assert.IsTrue(((IEnumerable<int>)(new int[] {})).SequenceEqual(new int[] {}));
      Assert.IsTrue(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).SequenceEqual(new[] {0, 1, 2, 3, 4}));
      //Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).EqualsAll(null));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).SequenceEqual(new int[] {}));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).SequenceEqual(new[] {0, 1, 2, 3}));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).SequenceEqual(new[] {0, 1, 2, 3, 4, 5}));
    }

    [Test]
    public void TestSequenceEqualWithIEqualityComparer()
    {
      Assert.IsTrue(((IEnumerable<string>)(new string[] {})).SequenceEqual(new string[] {}, StringComparer.OrdinalIgnoreCase));
      Assert.IsTrue(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).SequenceEqual(new[] {"A", "b", "C", "d", "E"}, StringComparer.OrdinalIgnoreCase));
      //Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).EqualsAll(null, StringComparer.OrdinalIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).SequenceEqual(new string[] {}, StringComparer.OrdinalIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).SequenceEqual(new[] {"A", "b", "C", "d"}, StringComparer.OrdinalIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).SequenceEqual(new[] {"A", "b", "C", "d", "E", "f"}, StringComparer.OrdinalIgnoreCase));
    }

    [Test]
    public void TestSelect()
    {
      Assert.IsTrue(((IEnumerable<string>)(new string[] {})).SequenceEqual((new int[] {}).Select(delegate(int i) {
        return i.ToString();
      })));
      Assert.IsTrue(((IEnumerable<string>)(new string[] {"0", "1", "2", "3", "4"})).SequenceEqual((new int[] {0, 1, 2, 3, 4}).Select(delegate(int i) {
        return i.ToString();
      })));
    }

    [Test]
    public void TestCast()
    {
      var collection = new System.Collections.ArrayList();

      collection.Add(1);
      collection.Add(2);
      collection.Add(3);

      Assert.IsTrue(collection.Cast<int>().SequenceEqual(new int[] {1, 2, 3}));
    }

    [Test]
    public void TestCount()
    {
      Assert.AreEqual(5, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Count());
      Assert.AreEqual(5, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Count());
      //Assert.AreEqual(5, ((IEnumerable)new ArrayList(new[] {0, 1, 2, 3, 4})).Count());
    }

    [Test]
    public void TestCount2()
    {
string[] fruits = { "apple", "banana", "mango", "orange", "passionfruit", "grape" };

    int numberOfFruits = fruits.Count();

      Assert.AreEqual(6, numberOfFruits);
    }

    [Test]
    public void TestCountWithPredicate2()
    {
    Pet[] pets = { new Pet { Name="Barley", Vaccinated=true },
                   new Pet { Name="Boots", Vaccinated=false },
                   new Pet { Name="Whiskers", Vaccinated=false } };

      int numberUnvaccinated = pets.Count(p => p.Vaccinated == false);

      Assert.AreEqual(2, numberUnvaccinated);
    }

    private static IEnumerable<int> GetEnumerator()
    {
      yield return 0;
      yield return 1;
      yield return 2;
      yield return 3;
      yield return 4;
    }

    private static IEnumerable<int> GetEmptyEnumerator()
    {
      yield break;
    }

    [Test]
    public void TestContains()
    {
      Assert.IsTrue (((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).Contains(2));
      Assert.IsFalse(((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).Contains(9));
      Assert.IsTrue (GetEnumerator().Contains(2));
      Assert.IsFalse(GetEnumerator().Contains(9));
      Assert.IsFalse(GetEmptyEnumerator().Contains(2));
      Assert.IsFalse(GetEmptyEnumerator().Contains(9));
    }

    [Test]
    public void TestContains2()
    {
string[] fruits = { "apple", "banana", "mango", "orange", "passionfruit", "grape" };

string fruit = "mango";

bool hasMango = fruits.Contains(fruit);

      Assert.IsTrue(hasMango);
    }

    [Test]
    public void TestContainsWithIEqualityComparer()
    {
      Assert.IsFalse(((IEnumerable<string>)(new string[] {})).Contains("a", StringComparer.OrdinalIgnoreCase));
      Assert.IsTrue (((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).Contains("a", StringComparer.OrdinalIgnoreCase));
      Assert.IsTrue (((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).Contains("A", StringComparer.OrdinalIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).Contains("x", StringComparer.OrdinalIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).Contains("X", StringComparer.OrdinalIgnoreCase));
    }

    [Test]
    public void TestEmpty()
    {
      Assert.AreEqual(0, Enumerable.Empty<int>().Count());
      CollectionAssert.AreEquivalent(new int[0], Enumerable.Empty<int>());
      CollectionAssert.AreEquivalent(GetEmptyEnumerator(), Enumerable.Empty<int>());
      CollectionAssert.AreEquivalent(new int[0], Enumerable.Empty<int>().ToArray());
    }

    [Test]
    public void TestFirst()
    {
      Assert.AreEqual(0, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).First());
      Assert.AreEqual(0, GetEnumerator().First());

      try {
        ((IEnumerable<int>)new int[] {}).First();
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }

      try {
        GetEmptyEnumerator().First();
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }
    }

    [Test]
    public void TestFirstOrDefault()
    {
      Assert.AreEqual(0, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).FirstOrDefault());
      Assert.AreEqual(0, GetEnumerator().FirstOrDefault());
      Assert.AreEqual(0, ((IEnumerable<int>)new int[] {}).FirstOrDefault());
      Assert.AreEqual(0, GetEmptyEnumerator().FirstOrDefault());
    }

    [Test]
    public void TestFirstOrDefault2()
    {
int[] numbers = { };
int first = numbers.FirstOrDefault();

      Assert.AreEqual(first, 0);
    }

    [Test]
    public void TestFirstOrDefaultWithPredicate()
    {
      Assert.AreEqual("a",   (new[] {"a", "aa", "aaa"}).FirstOrDefault(delegate(string s) {return s.Length == 1;}));
      Assert.AreEqual("aa",  (new[] {"a", "aa", "aaa"}).FirstOrDefault(delegate(string s) {return s.Length == 2;}));
      Assert.AreEqual("aaa", (new[] {"a", "aa", "aaa"}).FirstOrDefault(delegate(string s) {return s.Length == 3;}));
      Assert.AreEqual(null,  (new[] {"a", "aa", "aaa"}).FirstOrDefault(delegate(string s) {return s.Length == 4;}));

      Assert.AreEqual(3, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).FirstOrDefault(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).FirstOrDefault(delegate(int i){ return i == 9; }));
      Assert.AreEqual(3, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).FirstOrDefault(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).FirstOrDefault(delegate(int i){ return i == 9; }));
      Assert.AreEqual(3, ((IEnumerable<int>)GetEnumerator()).FirstOrDefault(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)GetEnumerator()).FirstOrDefault(delegate(int i){ return i == 9; }));
    }

    [Test]
    public void TestFirstOrDefaultWithPredicate2()
    {
string[] names = { "Hartono, Tommy", "Adams, Terry", 
                     "Andersen, Henriette Thaulow", 
                     "Hedlund, Magnus", "Ito, Shu" };

string firstLongName = names.FirstOrDefault(name => name.Length > 20);

      Assert.AreEqual("Andersen, Henriette Thaulow", firstLongName);

string firstVeryLongName = names.FirstOrDefault(name => name.Length > 30);

      Assert.IsNull(firstVeryLongName);
    }

    [Test]
    public void TestLast()
    {
      Assert.AreEqual(4, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).Last());
      Assert.AreEqual(4, GetEnumerator().Last());

      Assert.Throws<InvalidOperationException>(() => ((IEnumerable<int>)new int[] {}).Last());
      Assert.Throws<InvalidOperationException>(() => GetEmptyEnumerator().Last());
    }

    [Test]
    public void TestLastOrDefault()
    {
      Assert.AreEqual(4, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).LastOrDefault());
      Assert.AreEqual(4, GetEnumerator().LastOrDefault());
      Assert.AreEqual(0, ((IEnumerable<int>)new int[] {}).LastOrDefault());
      Assert.AreEqual(0, GetEmptyEnumerator().LastOrDefault());
    }

    [Test]
    public void TestLastOrDefault2()
    {
      int[] numbers = { };
      int last = numbers.LastOrDefault();

      Assert.AreEqual(last, 0);
    }

    [Test]
    public void TestLastOrDefaultWithPredicate()
    {
      Assert.AreEqual("a",   (new[] {"a", "aa", "aaa"}).LastOrDefault(delegate(string s) {return s.Length == 1;}));
      Assert.AreEqual("aa",  (new[] {"a", "aa", "aaa"}).LastOrDefault(delegate(string s) {return s.Length == 2;}));
      Assert.AreEqual("aaa", (new[] {"a", "aa", "aaa"}).LastOrDefault(delegate(string s) {return s.Length == 3;}));
      Assert.AreEqual(null,  (new[] {"a", "aa", "aaa"}).LastOrDefault(delegate(string s) {return s.Length == 4;}));

      Assert.AreEqual(3, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).LastOrDefault(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).LastOrDefault(delegate(int i){ return i == 9; }));
      Assert.AreEqual(3, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).LastOrDefault(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).LastOrDefault(delegate(int i){ return i == 9; }));
      Assert.AreEqual(3, ((IEnumerable<int>)GetEnumerator()).LastOrDefault(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)GetEnumerator()).LastOrDefault(delegate(int i){ return i == 9; }));
    }

    [Test]
    public void TestLastOrDefaultWithPredicate2()
    {
      double[] numbers = { 49.6, 52.3, 51.0, 49.4, 50.2, 48.3 };

      double last50 = numbers.LastOrDefault(n => Math.Round(n) == 50.0);

      Assert.AreEqual(50.2, last50);

      double last40 = numbers.LastOrDefault(n => Math.Round(n) == 40.0);

      Assert.AreEqual(0.0, last40);
    }

    [Test]
    public void TestWhere()
    {
      CollectionAssert.AreEquivalent(new int[0], ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Where(delegate(int i){ return i == 9; }));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Where(delegate(int i){ return i < 3; }));
    }

    [Test]
    public void TestWhere2()
    {
List<string> fruits =
    new List<string> { "apple", "passionfruit", "banana", "mango", 
                    "orange", "blueberry", "grape", "strawberry" };

IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6);

      Assert.IsTrue(query.SequenceEqual(new[] {"apple", "mango", "grape"}));
    }

    [Test]
    public void TestAny()
    {
      Assert.IsTrue (((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Any());
      Assert.IsFalse(((IEnumerable<int>)new int[0]).Any());
      Assert.IsTrue (GetEnumerator().Any());
      Assert.IsFalse(GetEmptyEnumerator().Any());
    }

    [Test]
    public void TestAnyWithPredicate()
    {
      Assert.IsTrue (((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Any(delegate(int i){ return i == 3; }));
      Assert.IsFalse(((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Any(delegate(int i){ return i == 9; }));
      Assert.IsTrue (((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Any(delegate(int i){ return i == 3; }));
      Assert.IsFalse(((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Any(delegate(int i){ return i == 9; }));
      Assert.IsTrue (((IEnumerable<int>)GetEnumerator()).Any(delegate(int i){ return i == 3; }));
      Assert.IsFalse(((IEnumerable<int>)GetEnumerator()).Any(delegate(int i){ return i == 9; }));
    }

    [Test]
    public void TestAnyWithPredicate2()
    {
    // Create an array of Pets.
    Pet[] pets =
        { new Pet { Name="Barley", Age=8, Vaccinated=true },
          new Pet { Name="Boots", Age=4, Vaccinated=false },
          new Pet { Name="Whiskers", Age=1, Vaccinated=false } };

    // Determine whether any pets over age 1 are also unvaccinated.
    bool unvaccinated =
        pets.Any(p => p.Age > 1 && p.Vaccinated == false);

      Assert.IsTrue(unvaccinated);
    }

    [Test]
    public void TestTake()
    {
      CollectionAssert.AreEquivalent(new int[0], ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(-1));
      CollectionAssert.AreEquivalent(new int[0], ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(0));
      CollectionAssert.AreEquivalent(new[] {0}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(1));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(3));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2, 3, 4}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(5));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2, 3, 4}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(10));
    }

    [Test]
    public void TestReverse()
    {
      CollectionAssert.AreEquivalent(new[] {4, 3, 2, 1, 0}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Reverse());
      CollectionAssert.AreEquivalent(new[] {4, 3, 2, 1, 0}, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Reverse());
      CollectionAssert.AreEquivalent(new[] {4, 3, 2, 1, 0}, ((IEnumerable<int>)GetEnumerator()).Reverse());
    }

    [Test]
    public void TestToArray()
    {
      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4}, 
                                              ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).ToArray()));
      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4}, 
                                              ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).ToArray()));
      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4}, 
                                              GetEnumerator().ToArray()));

      var dictionary = new Dictionary<string, int>() {
        {"0", 0},
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
      };

      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4},
                                              ((IEnumerable<int>)dictionary.Values).ToArray()));
    }

    [Test]
    public void TestToList()
    {
      var expected = new List<int>(new[] {0, 1, 2, 3, 4});

      CollectionAssert.AreEqual(expected, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).ToList());
      CollectionAssert.AreEqual(expected, GetEnumerator().ToList());

      Assert.AreNotSame(expected, ((IEnumerable<int>)expected).ToList());
      CollectionAssert.AreEqual(expected, ((IEnumerable<int>)expected).ToList());
    }

    [Test]
    public void TestAll()
    {
      Assert.IsTrue (((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).All(delegate(int i){ return 0 <= i; }));
      Assert.IsFalse(((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).All(delegate(int i){ return 3 <= i; }));
      Assert.IsTrue (((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).All(delegate(int i){ return 0 <= i; }));
      Assert.IsFalse(((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).All(delegate(int i){ return 3 <= i; }));
      Assert.IsTrue (((IEnumerable<int>)GetEnumerator()).All(delegate(int i){ return 0 <= i; }));
      Assert.IsFalse(((IEnumerable<int>)GetEnumerator()).All(delegate(int i){ return 3 <= i; }));

      Assert.IsTrue(((IEnumerable<int>)new int[] {}).All(delegate(int i){ return false; }));
      Assert.IsTrue(GetEmptyEnumerator().All(delegate(int i){ return false; }));
    }

    [Test]
    public void TestAll2()
    {
    Pet[] pets = { new Pet { Name="Barley", Age=10 },
                   new Pet { Name="Boots", Age=4 },
                   new Pet { Name="Whiskers", Age=6 } };

    // Determine whether all pet names 
    // in the array start with 'B'.
    bool allStartWithB = pets.All(pet =>
                                      pet.Name.StartsWith("B"));

      Assert.IsFalse(allStartWithB);
    }

    [Test]
    public void TestRange()
    {
      Assert.AreEqual(int.MaxValue, Enumerable.Range(int.MaxValue, 1).First());
      Assert.AreEqual(1, Enumerable.Range(1, int.MaxValue).First());

      CollectionAssert.AreEqual(new int[] {1, 4, 9, 16, 25, 36, 49, 64, 81, 100},
                                Enumerable.Range(1, 10).Select(x => x * x).ToArray());
    }

    [Test]
    public void TestRangeArgumentOutOfRange()
    {
      try {
        Enumerable.Range(0, -1).First();
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        Enumerable.Range(int.MaxValue, 2).First();
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        Enumerable.Range(2, int.MaxValue).First();
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }
    }
  }
}
