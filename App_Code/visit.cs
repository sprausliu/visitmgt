using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitMgt
{
    /// <summary>
    /// visit 的摘要说明
    /// </summary>
    public class VisitMain
    {
        public Guid ID { get; set; }
        public Gate Gate { get; set; }
        public VisitCard Card { get; set; }
        public int Life { get; set; }
        public Visitor Visitor { get; set; }
        public bool HasID { get; set; }
        public string vType { get; set; }
        public string vReason { get; set; }
        public int Nums { get; set; }
        public string Entry { get; set; }
        public Respondent Respondent { get; set; }
        public string status { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public VisitMain() { }
    }
    public class Plant
    {
        public string Code { get; set; }
        public string CNName { get; set; }
        public string ENName { get; set; }
        public string Address { get; set; }
        public Plant() { }
    }
    public class Gate
    {
        public int GateID { get; set; }
        public string Name { get; set; }
        public Plant Plant { get; set; }
        public Gate() { }
    }
    public class VisitCard 
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public CardType Type { get; set; }
        public VisitCard() {
        }
    }
    public enum CardType
    {
        NORMAL,VIP
    }
    public class Respondent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Plant Plant { get; set; }
        public string DEPT { get; set; }
        public string Tel { get; set; }
        public string Tel2 { get; set; }
        public Respondent() { }
    }
    public class CertType
    {
        public string TypeCode { get; set; }
        public string TypeDesc { get; set; }
        public CertType() { 
        }
        public CertType(string ct) {
            TypeCode = ct;
        }
    }
    public class Cert
    {
        public Guid CertID { get; set; }
        public Visitor Host { get; set; }
        public string Name { get; set; }
        public CertType CertType { get; set; }
        public string CertCode { get; set; }
        public DateTime ExpiredDate { get; set; }
        public Cert() { }
        public Cert(CertType ct,Visitor v) {
            CertID = Guid.NewGuid();
            Host = v;
            CertType = ct;
        }
    }
    public class Visitor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime LastFireTrain { get; set; }
        public Visitor() { 
            
        }
        public void FireTraining() {
            FireTrain ft = new FireTrain { Guid = Guid.NewGuid(), People = this, TrainDate = DateTime.Now };
            LastFireTrain = ft.TrainDate;
            //ft.save();
        }
        public Cert CreateNewCert(CertType ct) {
            return new Cert(ct, this);
        }
        public Cert CreateNewCert(string ct) {
            CertType CT = new CertType(ct);
            return new Cert(CT,this);
        }
    }
    public class FireTrain 
    {
        public Guid Guid { get; set; }
        public Visitor People { get; set; }
        public DateTime TrainDate { get; set; }
        public void save() { }
    }
    public enum Sex
	{
	    Male,Female,Other
	}

}
