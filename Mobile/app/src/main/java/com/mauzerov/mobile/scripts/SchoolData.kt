package com.mauzerov.mobile.scripts

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

class SchoolData
{
    var Students : Table = mutableListOf()
    var Teachers : Table = mutableListOf()
    var Employees : Table = mutableListOf()

    operator fun get(name: String) : Table? {
            return when (name.lowercase()) {
                "student" , "students" -> Students
                "teacher" , "teachers" -> Teachers
                "employee" , "employees" -> Employees
                else -> null
            }
    }

    operator fun set(name: String, value: Table) {
        when (name.lowercase()) {
            "student" , "students" -> Students = value
            "teacher" , "teachers" -> Teachers = value
            "employee" , "employees" -> Employees = value
            else -> throw NotImplementedError("Lack Of Tables Of Name: $name")
        }
    }

    fun tables() : List<String> {
        return this::class.java.declaredFields.iterator().asSequence().map { f -> f.name }.toList()
    }
}
