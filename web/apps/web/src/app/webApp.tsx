import { Suspense, lazy } from "react";
import { Route, Routes } from "react-router";
import SignedInGuard from "../components/auth/SignedInGuard";
import { SplashScreen } from "../components/common/SplashScreen";
import { UnauthenticatedLayout } from "../components/common/UnauthenticatedLayout";
import { SignInPage } from "../pages/signin";
import { SignUpPage } from "../pages/signup";
import { signInRoute, signUpRoute } from "../routes";

export default function WebApp() {
    return (
        <Routes>
            <Route>
                <Route element={<UnauthenticatedLayout />}>
                    <Route element={<SignInPage />} path={signInRoute} />
                    <Route element={<SignUpPage />} path={signUpRoute} />
                </Route>
            </Route>

            <Route element={<SignedInGuard />}>
                <Route
                    element={
                        <Suspense fallback={<SplashScreen />}>
                            <AuthenticatedApp />
                        </Suspense>
                    }
                    path="*"
                />
            </Route>
        </Routes>
    );
}

const AuthenticatedApp = lazy(() => import("./authenticatedApp"));
