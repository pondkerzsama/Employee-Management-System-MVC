using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeSystem.Models;
using System.Collections.Generic;
using EmployeeSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystem.Controllers
{
    public class EmployeeController : Controller
    {
        // 1. ประกาศตัวแปรสำหรับเก็บ "ล่ามส่วนตัว" (DbContext)
        private readonly ApplicationDbContext _context;

        // 2. รับล่ามเข้ามาผ่าน Constructor (Dependency Injection ที่เราทำใน Program.cs)
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 4. หน้ากรอกข้อมูล (Create - GET): รอรับข้อมูล
        public IActionResult Create()
        {
            return View();
        }

        // 5. รับข้อมูลจากฟอร์ม (Create - POST): รับค่าที่ User กรอกแล้วบันทึก
        [HttpPost] // (สำหรับรับข้อมูลมาแสดงผล)
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            // เช็กว่าข้อมูลถูกต้องไหม (เทียบจาก model)
            if (ModelState.IsValid)
            {
                // บอกล่าม: "จดคนนี้ลงสมุดนะ" (ยังไม่บันทึกจริง แค่จดพักไว้)
                _context.Employees.Add(employee);
                
                // บอกล่าม: "บันทึกข้อมูลลง SQL เดี๋ยวนี้!" (Save จริงตรงนี้)
                _context.SaveChanges();

                // สั่งไปหน้า Index
                return RedirectToAction("Index");
            }

            // ถ้าข้อมูลผิด ให้เด้งกลับไปหน้าเดิม พร้อมแจ้ง Error
            return View(employee);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Find(id) ใช้ได้เฉพาะกับ Primary Key เท่านั้น
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0 )
            {
                return NotFound();
            }

            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // สังเกต Attribute [ActionName("Delete")] นะครับ 
        // เพราะใน C# เราตั้งชื่อฟังก์ชันซ้ำกันเป๊ะๆ (Delete รับ int เหมือนกัน) ไม่ได้ 
        // เราเลยต้องตั้งชื่อในโค้ดว่า DeleteConfirmed แต่หลอกหน้าเว็บว่าชื่อ "Delete"
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(string searchString)
        {   
            var employees = from e in _context.Employees
                            select e;
            
            if(!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.FirstName.Contains(searchString));   
            }

            return View(await employees.ToListAsync());
        }

    }
}