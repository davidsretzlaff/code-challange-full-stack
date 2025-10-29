"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"
import type { LoginCredentials } from "@/lib/auth"
import { apiClient } from "@/lib/api-client"
import { useToast } from "@/hooks/use-toast"

export function useAuth() {
  const [isLoading, setIsLoading] = useState(false)
  const router = useRouter()
  const { toast } = useToast()

  const login = async (credentials: LoginCredentials) => {
    setIsLoading(true)
    try {
      const { data } = await apiClient.post("/auth/login", {
        email: credentials.email,
        password: credentials.password,
      })

      const accessToken: string | undefined = data?.accessToken ?? data?.AccessToken
      const expiresInSeconds: number | undefined = data?.expiresInSeconds ?? data?.ExpiresInSeconds

      if (accessToken) {
        localStorage.setItem("auth_token", accessToken)
        localStorage.setItem("user_email", credentials.email)

        if (typeof expiresInSeconds === "number" && Number.isFinite(expiresInSeconds)) {
          const expiresAt = Date.now() + expiresInSeconds * 1000
          localStorage.setItem("auth_expires_at", String(expiresAt))
        }

        toast({
          title: "Login realizado com sucesso",
          description: "Bem-vindo de volta!",
        })

        router.push("/")
        return
      }

      toast({
        title: "Falha no login",
        description: "Credenciais inválidas",
        variant: "destructive",
      })
    } catch (error) {
      toast({
        title: "Erro",
        description: "Ocorreu um erro durante o login",
        variant: "destructive",
      })
    } finally {
      setIsLoading(false)
    }
  }

  const logout = () => {
    if (typeof window !== "undefined") {
      localStorage.removeItem("auth_token")
      localStorage.removeItem("user_email")
      localStorage.removeItem("auth_expires_at")
    }
    toast({
      title: "Sessão encerrada",
      description: "Você saiu com sucesso",
    })
    router.push("/login")
  }

  const isAuthenticated = (): boolean => {
    if (typeof window === "undefined") return false
    const token = localStorage.getItem("auth_token")
    if (!token) return false
    const expiresAt = localStorage.getItem("auth_expires_at")
    if (!expiresAt) return true
    const exp = Number(expiresAt)
    if (Number.isFinite(exp) && exp > Date.now()) return true
    // Expirado: limpar
    localStorage.removeItem("auth_token")
    localStorage.removeItem("auth_expires_at")
    return false
  }

  return {
    login,
    logout,
    isLoading,
    isAuthenticated: isAuthenticated(),
  }
}
