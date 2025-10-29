"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { mockEmployeeApi } from "@/lib/mock-data"
import type { CreateEmployeeDto, UpdateEmployeeDto } from "@/lib/types"
import { useToast } from "@/hooks/use-toast"
import { employeeApi } from "@/lib/api-client"

const EMPLOYEES_QUERY_KEY = ["employees"]

export function useEmployees() {
  return useQuery({
    queryKey: EMPLOYEES_QUERY_KEY,
    queryFn: employeeApi.getAll,
  })
}

export function useEmployee(id: number) {
  return useQuery({
    queryKey: [...EMPLOYEES_QUERY_KEY, id],
    queryFn: () => employeeApi.getById(id),
    enabled: !!id,
  })
}

export function useCreateEmployee() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  return useMutation({
    mutationFn: (employee: CreateEmployeeDto) => employeeApi.create(employee),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: EMPLOYEES_QUERY_KEY })
      toast({
        title: "Success",
        description: "Employee created successfully",
      })
    },
    onError: (error: Error) => {
      toast({
        title: "Error",
        description: error.message || "Failed to create employee",
        variant: "destructive",
      })
    },
  })
}

export function useUpdateEmployee() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateEmployeeDto }) => employeeApi.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: EMPLOYEES_QUERY_KEY })
      toast({
        title: "Success",
        description: "Employee updated successfully",
      })
    },
    onError: (error: Error) => {
      toast({
        title: "Error",
        description: error.message || "Failed to update employee",
        variant: "destructive",
      })
    },
  })
}

export function useDeleteEmployee() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  return useMutation({
    mutationFn: (id: number) => employeeApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: EMPLOYEES_QUERY_KEY })
      toast({
        title: "Success",
        description: "Employee deleted successfully",
      })
    },
    onError: (error: Error) => {
      toast({
        title: "Error",
        description: error.message || "Failed to delete employee",
        variant: "destructive",
      })
    },
  })
}
