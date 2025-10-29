"use client"

import { useState } from "react"
import { Plus, Users, Loader2, LogOut } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card } from "@/components/ui/card"
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { EmployeeList } from "@/components/employee-list"
import { EmployeeForm } from "@/components/employee-form"
import { AuthGuard } from "@/components/auth-guard"
import { useAuth } from "@/hooks/use-auth"
import { useEmployees, useCreateEmployee, useUpdateEmployee, useDeleteEmployee } from "@/hooks/use-employees"
import type { Employee, CreateEmployeeDto } from "@/lib/types"

export default function HomePage() {
  return (
    <AuthGuard>
      <DashboardContent />
    </AuthGuard>
  )
}

function DashboardContent() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingEmployee, setEditingEmployee] = useState<Employee | null>(null)
  const { logout } = useAuth()

  const { data: employees, isLoading, error } = useEmployees()
  const createMutation = useCreateEmployee()
  const updateMutation = useUpdateEmployee()
  const deleteMutation = useDeleteEmployee()

  const handleCreate = (data: CreateEmployeeDto) => {
    createMutation.mutate(data, {
      onSuccess: () => {
        setIsDialogOpen(false)
      },
    })
  }

  const handleUpdate = (data: CreateEmployeeDto) => {
    if (editingEmployee) {
      updateMutation.mutate(
        { id: editingEmployee.id, data },
        {
          onSuccess: () => {
            setIsDialogOpen(false)
            setEditingEmployee(null)
          },
        },
      )
    }
  }

  const handleEdit = (employee: Employee) => {
    setEditingEmployee(employee)
    setIsDialogOpen(true)
  }

  const handleDelete = (id: number) => {
    deleteMutation.mutate(id)
  }

  const handleDialogClose = () => {
    setIsDialogOpen(false)
    setEditingEmployee(null)
  }

  return (
    <main className="min-h-screen bg-background">
      <div className="container mx-auto px-4 py-8 max-w-7xl">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-3">
            <div className="rounded-lg bg-primary/10 p-2">
              <Users className="h-6 w-6 text-primary" />
            </div>
            <div>
              <h1 className="text-3xl font-bold tracking-tight">Employee Management</h1>
              <p className="text-muted-foreground">Manage your team members and their information</p>
            </div>
          </div>
          <div className="flex gap-3">
            <Button variant="outline" onClick={logout}>
              <LogOut className="h-4 w-4 mr-2" />
              Logout
            </Button>
            <Button onClick={() => setIsDialogOpen(true)} size="lg">
              <Plus className="h-4 w-4 mr-2" />
              Add Employee
            </Button>
          </div>
        </div>

        {/* Stats Card */}
        {employees && Array.isArray(employees) && (
          <Card className="p-6 mb-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-muted-foreground">Total Employees</p>
                <p className="text-3xl font-bold">{employees.length}</p>
              </div>
              <div className="rounded-full bg-primary/10 p-3">
                <Users className="h-6 w-6 text-primary" />
              </div>
            </div>
          </Card>
        )}

        {/* Content */}
        {isLoading && (
          <Card className="p-12">
            <div className="flex flex-col items-center gap-4">
              <Loader2 className="h-8 w-8 animate-spin text-primary" />
              <p className="text-muted-foreground">Loading employees...</p>
            </div>
          </Card>
        )}

        {error && (
          <Card className="p-12 border-destructive">
            <div className="text-center">
              <h3 className="text-lg font-semibold text-destructive mb-2">Error Loading Data</h3>
              <p className="text-sm text-muted-foreground">
                {error instanceof Error ? error.message : "Failed to load employees"}
              </p>
            </div>
          </Card>
        )}

        {employees && Array.isArray(employees) && (
          <EmployeeList
            employees={employees}
            onEdit={handleEdit}
            onDelete={handleDelete}
            isDeleting={deleteMutation.isPending}
          />
        )}

        {/* Dialog for Create/Edit */}
        <Dialog open={isDialogOpen} onOpenChange={handleDialogClose}>
          <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>{editingEmployee ? "Edit Employee" : "Add New Employee"}</DialogTitle>
              <DialogDescription>
                {editingEmployee
                  ? "Update the employee information below"
                  : "Fill in the details to add a new employee"}
              </DialogDescription>
            </DialogHeader>
            <EmployeeForm
              employee={editingEmployee || undefined}
              onSubmit={editingEmployee ? handleUpdate : handleCreate}
              onCancel={handleDialogClose}
              isLoading={createMutation.isPending || updateMutation.isPending}
            />
          </DialogContent>
        </Dialog>
      </div>
    </main>
  )
}
