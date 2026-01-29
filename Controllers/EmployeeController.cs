using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeSystem.Models;
using System.Collections.Generic;
using EmployeeSystem.Data;

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

        // 3. หน้าแรก (Index): แสดงรายชื่อพนักงานทั้งหมด
        public IActionResult Index()
        {
            // สั่งล่าม: "ไปเอา Employees ทั้งหมดมาแปลงเป็น List ให้หน่อย"
            var allEmployees = _context.Employees.ToList();
            
            // ส่งข้อมูล (Model) ไปให้หน้าเว็บ (View) แสดงผล
            return View(allEmployees);
        }

        // 4. หน้ากรอกข้อมูล (Create - GET): แค่เปิดหน้าฟอร์มเปล่าๆ ขึ้นมา
        public IActionResult Create()
        {
            return View();
        }

        // 5. รับข้อมูลจากฟอร์ม (Create - POST): รับค่าที่ User กรอกแล้วบันทึก
        [HttpPost] // บอกว่าอันนี้รับของนะ ไม่ใช่แค่ขอหน้าเว็บ
        public IActionResult Create(Employee employee)
        {
            // เช็กว่าข้อมูลถูกต้องไหม (เช่น ลืมกรอกชื่อไหม? เงินเดือนติดลบไหม?)
            if (ModelState.IsValid)
            {
                // บอกล่าม: "จดคนนี้ลงสมุดนะ" (ยังไม่บันทึกจริง แค่จดพักไว้)
                _context.Employees.Add(employee);
                
                // บอกล่าม: "บันทึกข้อมูลลง SQL เดี๋ยวนี้!" (Save จริงตรงนี้)
                _context.SaveChanges();

                // บันทึกเสร็จแล้ว เด้งกลับไปหน้า Index
                return RedirectToAction("Index");
            }

            // ถ้าข้อมูลผิด (เช่น ลืมกรอกชื่อ) ให้เด้งกลับไปหน้าเดิม พร้อมแจ้ง Error
            return View(employee);
        }
    }
}