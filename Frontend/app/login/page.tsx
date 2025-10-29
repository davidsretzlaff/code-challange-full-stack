import { Shield } from "lucide-react"
import { Card } from "@/components/ui/card"
import { LoginForm } from "@/components/login-form"

export default function LoginPage() {
  return (
    <div className="min-h-screen bg-background flex items-center justify-center p-4">
      <div className="w-full max-w-md space-y-8">
        {/* Logo and Header */}
        <div className="text-center space-y-2">
          <div className="flex justify-center mb-4">
            <div className="rounded-2xl bg-primary/10 p-4">
              <Shield className="h-10 w-10 text-primary" />
            </div>
          </div>
          <h1 className="text-3xl font-bold tracking-tight">Welcome back</h1>
          <p className="text-muted-foreground">Sign in to your employee management account</p>
        </div>

        {/* Login Card */}
        <Card className="p-8 shadow-lg">
          <LoginForm />
        </Card>

        {/* Footer */}
        <p className="text-center text-xs text-muted-foreground">Secure employee management system</p>
      </div>
    </div>
  )
}
