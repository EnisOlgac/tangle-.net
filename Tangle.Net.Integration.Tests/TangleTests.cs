﻿namespace Tangle.Net.Integration.Tests
{
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using RestSharp;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository;
  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The tangle tests.
  /// </summary>
  [TestClass]
  public class TangleTests
  {
    #region Fields

    /// <summary>
    /// The repository.
    /// </summary>
    private RestIotaRepository repository;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The setup.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
      this.repository = new RestIotaRepository(new RestClient("http://localhost:14265"));
    }

    /// <summary>
    /// The test get latest inclusion.
    /// </summary>
    [TestMethod]
    public void TestGetLatestInclusion()
    {
      var inclusionState = this.repository.GetLatestInclusion(
          new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") });

      Assert.IsTrue(inclusionState.States.First(k => k.Key.Value == "HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999").Value);
    }

    /// <summary>
    /// The test find transactions.
    /// </summary>
    [TestMethod]
    public void TestFindTransactions()
    {
      var transactions =
        this.repository.FindTransactionsByAddresses(
          new List<Address> { new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9") });

      Assert.IsTrue(transactions.Hashes.Any());

      transactions =
        this.repository.FindTransactionsByBundles(
          new List<Hash> { new Hash("JSHICEYJKLEQLBNR9ZFJ9KIZUNQSGAI9DRZXONQJFZKETCHWCZWD9JMIAFAGDSOVFKIBOSRXY9ZKKFXWD") });

      Assert.IsTrue(transactions.Hashes.Any());

      transactions =
        this.repository.FindTransactionsByApprovees(
          new List<Hash> { new Hash("AYZMIHSFSKIKPUUUBENOIBSEVBGOCBVGAIPRWHNEFHBROZIKKYXXZDPVKJHIUSANFPLDIUBKFUPSA9999") });

      Assert.IsTrue(transactions.Hashes.Any());

      transactions =
        this.repository.FindTransactions(
          new Dictionary<string, IEnumerable<TryteString>>
            {
              {
                "addresses", 
                new List<TryteString>
                  {
                    new TryteString(
                      "GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9")
                  }
              }, 
              {
                "bundles", 
                new List<TryteString>
                  {
                    new TryteString(
                      "JSHICEYJKLEQLBNR9ZFJ9KIZUNQSGAI9DRZXONQJFZKETCHWCZWD9JMIAFAGDSOVFKIBOSRXY9ZKKFXWD")
                  }
              }, 
              {
                "approvees", 
                new List<TryteString>
                  {
                    new TryteString(
                      "AYZMIHSFSKIKPUUUBENOIBSEVBGOCBVGAIPRWHNEFHBROZIKKYXXZDPVKJHIUSANFPLDIUBKFUPSA9999")
                  }
              }
            });

      Assert.IsTrue(transactions.Hashes.Any());
    }

    /// <summary>
    /// The test get balances.
    /// </summary>
    [TestMethod]
    public void TestGetBalances()
    {
      var balances =
        this.repository.GetBalances(
          new List<Address>
            {
              new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9"), 
              new Address("HBBYKAKTILIPVUKFOTSLHGENPTXYBNKXZFQFR9VQFWNBMTQNRVOUKPVPRNBSZVVILMAFBKOTBLGLWLOHQ999999999")
            });

      Assert.IsTrue(balances.Addresses.Any());
      Assert.AreEqual(99500000, balances.Addresses[0].Balance);
    }

    /// <summary>
    /// The test get bundle.
    /// </summary>
    [TestMethod]
    public void TestGetBundle()
    {
      var bundle = this.repository.GetBundle(new Hash("J9GYFNZBGUGDDODHXUXXVI9AVFSAWVCVQXXSOXXVQATVLYDNMRKNTKYLVXDWENTSBN9XXGCARD9B99999"));

      Assert.AreEqual(15, bundle.Transactions.Count);
    }

    /// <summary>
    /// The test get inclusion states.
    /// </summary>
    [TestMethod]
    public void TestGetInclusionStates()
    {
      var tips = this.repository.GetTips();
      var inclusionsStates =
        this.repository.GetInclusionStates(
          new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") }, 
          tips.Hashes.GetRange(0, 1));

      Assert.IsTrue(
        inclusionsStates.States.First(entry => entry.Key.Value == "HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999")
          .Value);
    }

    /// <summary>
    /// The test get new addresses.
    /// </summary>
    [TestMethod]
    public void TestGetNewAddresses()
    {
      var newAddresses = this.repository.GetNewAddresses(Seed.Random(), 0, 2, SecurityLevel.Medium);
      Assert.AreEqual(2, newAddresses.Count);
    }

    /// <summary>
    /// The test get tips.
    /// </summary>
    [TestMethod]
    public void TestGetTips()
    {
      var tips = this.repository.GetTips();
      Assert.IsTrue(tips.Hashes.Any());
    }

    /// <summary>
    /// The test get trytes.
    /// </summary>
    [TestMethod]
    public void TestGetTrytes()
    {
      var transactionTrytes =
        this.repository.GetTrytes(new List<Hash> { new Hash("NHQZUPEIVHWVQLZIRSGPYMVOPWOWZTGZOYSUGBSOQSQNKZARGMXADKSDSMUSUZNFIQGURGEMQYGFZ9999") });

      var transaction = Transaction.FromTrytes(transactionTrytes[0]);

      Assert.AreEqual("YTXCUUWTXIXVRQIDSECVFRTKAFOEZITGDPLWYVUVFURMNVDPIRXEIQN9JHNFNVKVJMQVMA9GDZJROTSFZ", transaction.Address.Value);

      var bundle = new Bundle();
      bundle.Transactions.Add(transaction);
      var messages = bundle.GetMessages();

      Assert.AreEqual("Hello world!", messages[0]);
    }

    /// <summary>
    /// The test attach to tangle.
    /// </summary>
    [TestMethod]
    public void TestSendTrytes()
    {
      var seed = Seed.Random();
      var bundle = new Bundle();
      bundle.AddTransaction(
        new Transfer
          {
            Address =
              new Address("YTXCUUWTXIXVRQIDSECVFRTKAFOEZITGDPLWYVUVFURMNVDPIRXEIQN9JHNFNVKVJMQVMA9GDZJROTSFZHIVJOVAEC") { Balance = 0 }, 
            Message = TryteString.FromString("Hello world!"), 
            Tag = new Tag("CSHARP"), 
            Timestamp = Timestamp.UnixSecondsTimestamp
          });

      bundle.Finalize();
      bundle.Sign(new KeyGenerator(seed));

      var resultTransactions = this.repository.SendTrytes(bundle.Transactions);

      Assert.IsTrue(resultTransactions.Any());
    }

    #endregion
  }
}