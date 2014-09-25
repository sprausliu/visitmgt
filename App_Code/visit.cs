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
        public bool loaded { get; set; }
        public VisitMain() {
            this.ID = Guid.NewGuid();
            this.loaded = false;
        }
        protected void save() {
            try
            {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    visit_main vm = new visit_main
                    {
                        v_id=this.ID,
                        v_gate=this.Gate.GateID,
                        v_card=this.Card.Id,
                        v_life=this.Life,
                        v_visitor=this.Visitor.Id,
                        v_hasID=this.HasID,
                        v_type=this.vType,
                        v_reason=this.vReason,
                        v_nums=this.Nums,
                        v_entry=this.Entry,
                        v_respondent=this.Respondent.Id,
                        v_status=this.status,
                        v_intime=this.InTime,
                        v_outtime=this.OutTime
                    };
                    vmdc.visit_main.InsertOnSubmit(vm);
                    vmdc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("保存访问记录失败",ex);
            }
        }
        public void IssueCard(Gate g,VisitCard c,Visitor v,Respondent rp,bool hi,int life,string t,string r,int n,string e,string s) {
            this.Gate = g;
            this.Card = c;
            this.Life = life;
            this.Visitor = v;
            this.HasID = hi;
            this.vType = t;
            this.vReason = r;
            this.Nums = n;
            this.Entry = e;
            this.Respondent = rp;
            this.status = s;
            this.InTime = DateTime.Now;
            c.ChangeStatus(false);
            save();
        }
        public void Load() {
            if (this.ID!=null)
            {
                try
                {
                    using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                    {
                        visit_main vm = vmdc.visit_main.Single(q => q.v_id == this.ID);
                        this.Gate = new Gate(vm.v_gate);
                        this.Card = new VisitCard(vm.v_card);
                        this.Life = vm.v_life;
                        this.Visitor = new Visitor(vm.v_visitor);
                        this.HasID = vm.v_hasID;
                        this.vType = vm.v_type;
                        this.vReason = vm.v_reason;
                        this.Nums =(int)vm.v_nums;
                        this.Entry = vm.v_entry;
                        this.Respondent = new Respondent((int)vm.v_respondent);
                        this.status = vm.v_status;
                        this.loaded = true;
                        if (vm.v_intime!=null)
                        {
                            this.InTime = (DateTime)vm.v_intime;
                        }
                        if (vm.v_outtime!=null)
                        {
                            this.OutTime = (DateTime)vm.v_outtime;
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    throw new Exception("加载访问信息失败", ex);
                }
            }
            else
            {
                throw new Exception("未指定ID");
            }
        }
        public void CardBack() {
            if (!this.loaded)
            {
                this.Load();
            }
            this.OutTime = DateTime.Now;
            this.save();
        }
        public void Postponed(int i) {
            if (this.ID!=null)
            {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    visit_main vm = vmdc.visit_main.Single(q => q.v_id == this.ID);
                    vm.v_life += i;
                    vmdc.SubmitChanges();
                }
            }
        }
    }
    public class Plant
    {        
        public string Code { get; set; }
        public string CNName { get; set; }
        public string ENName { get; set; }
        public string Address { get; set; }
        public Plant() { }

        public Plant(string p)
        {
            this.Code = p;
        }
        public void Load() {
            if (this.Code!=null)
            {
                try
                {
                    using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                    {
                        plant po = vmdc.plant.Single(q => q.plant_code == this.Code);
                        this.CNName = po.plant_name;
                        this.ENName = po.plant_name_en;
                        this.Address = po.plant_address;
                    }
                }
                catch (Exception ex)
                {
                
                    throw new Exception("获取工厂信息失败",ex);
                }
            }
        }
        public void save(){
            try
            {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    plant po = new plant { plant_name = this.CNName, plant_name_en = this.ENName, plant_address = this.Address };
                    if (this.Code!=null)
                    {
                        po.plant_code = this.Code;
                    }
                    vmdc.plant.InsertOnSubmit(po);
                    vmdc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("更新工厂信息失败",ex);
            }
        }
        public void Delete() {
            if (this.Code!=null)
            {
                try
                {
                    using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                    {
                        vmdc.plant.DeleteOnSubmit(vmdc.plant.Single(q => q.plant_code == this.Code));
                        vmdc.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("删除工厂失败",ex);
                }
            }
        }
    }
    public class Gate
    {
        public int GateID { get; set; }
        public string Name { get; set; }
        public Plant Plant { get; set; }
        public Gate() { }

        public Gate(int p)
        {
            this.GateID = p;
        }
        public void Load()
        {
            if (this.GateID!=null)
            {
                try
                {
                    using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                    {
                        gate g = vmdc.gate.Single(q => q.gate_id == this.GateID);
                        this.GateID = this.GateID;
                        this.Name = g.gate_name;
                        this.Plant = new Plant(g.gate_plant);
                    }
                }
                catch (Exception ex)
                {
                
                    throw new Exception("获取门信息失败",ex);
                }
            }
        }
        public void save() {
            try
            {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    gate g = new gate {gate_plant = this.Plant.Code, gate_name = this.Name };
                    if (GateID != null) {
                        g.gate_id = GateID;
                    }
                    vmdc.gate.InsertOnSubmit(g);
                    vmdc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("更新门信息失败",ex);
            }
        }
        public void Delete() {
            if (GateID != null)
            {
                try
                {
                    using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                    {
                        vmdc.gate.DeleteOnSubmit(vmdc.gate.Single(q => q.gate_id == this.GateID));
                        vmdc.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception("删除门信息失败", ex);
                }
            }
        }
    }
    public class VisitCard 
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public CardType Type { get; set; }
        public bool availiable { get; set; }
        public VisitCard() {
        }

        public VisitCard(string p)
        {
            this.Id = p;
        }
        public void Load() {
            if (this.Id!=null)
            {
                try
                {
                    using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                    {
                        visit_card vc = vmdc.visit_card.Single(q => q.c_id == this.Id);
                        this.Desc = vc.c_desc;
                        this.availiable = (bool)vc.c_avaliable;
                        this.Type = (VisitMgt.CardType)vc.c_type;
                    }
                }
                catch (Exception ex)
                {
                
                    throw new Exception("获取访客卡信息失败",ex);
                }
            }
        }
        public void save() {
            try
            {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    visit_card vc = new visit_card
                    {
                        c_id = this.Id,
                        c_desc = this.Desc,
                        c_avaliable = this.availiable,
                        c_type = (int)this.Type
                    };
                    vmdc.visit_card.InsertOnSubmit(vc);
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("更新访客卡失败",ex);
            }
        }
        public void ChangeStatus(bool a) {
            this.availiable = a;
            this.save();
        }
    }
    public enum CardType
    {
        NORMAL=1,VIP=2
    }
    public class Respondent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Plant Plant { get; set; }
        public string DEPT { get; set; }
        public string Tel { get; set; }
        public string Tel2 { get; set; }
        private bool isnew { get; set; }
        public Respondent() {
            this.isnew = true;
        }
        public List<Respondent> QueryFromExt(string k) {
            List<Respondent> rl = new List<Respondent>();
            return rl;
        }
        public Respondent(int rid) {
            using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
            {
                var i = vmdc.respondent.Single(q => q.respondent_id==rid);
                this.Id = rid;
                this.Name = i.respondent_name;
                this.Plant = new Plant(i.respondent_plant);
                this.DEPT = i.respondent_dept;
                this.Tel = i.respondent_tel;
                this.Tel2 = i.respondent_tel2;
                this.isnew = false;
            }            
        }
        public void Delete() {
            if (this.Id != null) {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    vmdc.respondent.DeleteOnSubmit(vmdc.respondent.Single(q => q.respondent_id == this.Id));
                    vmdc.SubmitChanges();
                }
            }
        }
    }
    public class CertType
    {
        public string TypeCode { get; set; }
        public string TypeDesc { get; set; }
        public CertType() { 
        }
        private bool CheckCodeEmpty()
        {
            if (TypeCode == string.Empty)
            {
                throw new Exception("证件类型代码不能为空");
            }
            return true;
        }
        private bool CheckDescEmpty() 
        {
            if (TypeDesc == string.Empty)
            {
                throw new Exception("证件类型描述不能为空");
            }
            return true;
        }
        public CertType(string ct) {
            TypeCode = ct;
        }
        public void Create()
        {
            try
            {
                if (CheckCodeEmpty() && CheckDescEmpty())
                {
                    using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                    {
                        cert_type ct = new cert_type { cert_type_code = this.TypeCode, cert_type_desc = this.TypeDesc };
                        vmdc.cert_type.InsertOnSubmit(ct);
                        vmdc.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("创建证件更新失败", ex);
            }
        }
        public void Delete()
        {
            try
            {
                if (CheckCodeEmpty())
                {
                    using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                    {
                        cert_type ct = vmdc.cert_type.Single(q => q.cert_type_code == this.TypeCode);
                        vmdc.cert_type.DeleteOnSubmit(ct);
                        vmdc.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("删除证件类型失败", ex);
            }
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
        public Cert(Guid id){
            try
            {
                using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                {
                    visitor_cert vc = vmdc.visitor_cert.Single(q => q.cert_id == id);
                    this.CertID = id;
                    this.Host = new Visitor((int)vc.cert_host);
                    this.Name = vc.cert_name;
                    this.CertType = new CertType(vc.cert_type);
                    this.CertCode = vc.cert_code;
                    if (vc.cert_exipred != null)
                    {
                        this.ExpiredDate = (DateTime)vc.cert_exipred;
                    }
                }
            }
            catch(Exception ex) {
                throw new Exception("获取证件失败", ex);
            }
        }
        public Cert(CertType ct,Visitor v) {
            CertID = Guid.NewGuid();
            Host = v;
            CertType = ct;
        }
        public void Save() {
            try
            {
                using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                {
                    visitor_cert vc = new visitor_cert
                    {
                        cert_id = this.CertID,
                        cert_host = this.Host.Id,
                        cert_name = this.Name,
                        cert_type = this.CertType.TypeCode,
                        cert_code = this.CertCode,
                        cert_exipred = this.ExpiredDate
                    };
                    vmdc.visitor_cert.InsertOnSubmit(vc);
                }
            }
            catch (Exception ex) {
                throw new Exception("保存证件失败", ex);
            }
        }
        public void Delete() {
            try
            {
                using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                {
                    visitor_cert vc=vmdc.visitor_cert.Single(q => q.cert_id == CertID);
                    vmdc.visitor_cert.DeleteOnSubmit(vc);
                    vmdc.SubmitChanges();
                }
            }
        }
    }
    public class Visitor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime LastFireTrain { get; set; }        
        private bool isnew { get; set; }
        public Visitor() {
            isnew = true;
        }
        public Visitor(int id) {
            this.Id = id;
        }
        public void Load() {
            if (this.Id != null) {
                try
                {
                    using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                    {
                        visitor v = vmdc.visitor.Single(q => q.visitor_id == this.Id);
                        this.isnew = false;
                        this.Name = v.visitor_name;
                        this.Sex = (VisitMgt.Sex)Enum.Parse(typeof(VisitMgt.Sex), v.visitor_sex, true);
                        if (v.visitor_lastcheck != null)
                        {
                            this.LastFireTrain = (DateTime)v.visitor_lastcheck;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取访客信息失败", ex);
                }
            }
        }
        public void Save() {
            using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
            {
                visitor v=new visitor{
                    visitor_name=this.Name,
                    visitor_sex=this.Sex.ToString()
                };
                if (!isnew)
                {
                    v.visitor_id = this.Id;
                }
                vmdc.visitor.InsertOnSubmit(v);
                vmdc.SubmitChanges();
            }            
        }
        public void FireTraining() {
            FireTrain ft = new FireTrain(this);
            LastFireTrain = ft.TrainDate;
            ft.New();
        }
        public Cert CreateNewCert(CertType ct) {
            return new Cert(ct, this);
        }
        public Cert CreateNewCert(string ct) {
            CertType CT = new CertType(ct);
            return new Cert(CT,this);
        }
        public void DeleteCert(Guid certid) {
            Cert c = new Cert(certid);
            c.Delete();
        }
        public void DeleteCert(string certid) {
            DeleteCert(new Guid(certid));
        }
        public void Delete() {
            if (!isnew && Id != null) {
                try
                {
                    using (VisitMgtDataContext vmdc=new VisitMgtDataContext())
                    {
                        vmdc.visitor.DeleteOnSubmit(vmdc.visitor.Single(q => q.visitor_id == this.Id));
                        vmdc.visitor_cert.DeleteAllOnSubmit(vmdc.visitor_cert.Where(q => q.cert_host == this.Id));
                        vmdc.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    
                    throw new Exception("删除方可信息失败",ex);
                }
            }
        }

    }
    public class FireTrain 
    {
        public Guid Guid { get; set; }
        public Visitor People { get; set; }
        public DateTime TrainDate { get; set; }
        public FireTrain(Visitor v){
            Guid=Guid.NewGuid();
            People = v;
            TrainDate = DateTime.Now;
        }
        public void New() {
            try
            {
                using (VisitMgtDataContext vmdc = new VisitMgtDataContext())
                {
                    fire_train ft = new fire_train { train_id = Guid, visitor = this.People.Id, train_date = this.TrainDate };
                    vmdc.fire_train.InsertOnSubmit(ft);
                    visitor v = vmdc.visitor.Single(q => q.visitor_id == this.People.Id);
                    v.visitor_lastcheck = this.TrainDate;
                    vmdc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("培训失败", ex);
            }
        }
    }
    public enum Sex
	{
	    Male,Female,Other
	}

}
