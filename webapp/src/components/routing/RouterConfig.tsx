import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Layout } from "../share/Layout";
import { NotFound } from "../NotFound";
import { Login } from "../authentication/Login";
import { Home } from "../home/Home";
import { Logout } from '../authentication/Logout';
import { SignUp } from '../user/SignUp';
import { PrivatePage } from "../PrivatePage";
import { DeleteUser } from "../user/Delete";
import { ChangeName } from "../user/ChangeName";
import { ChangePassword } from "../user/ChangePassword";
import { EditUser } from "../user/Edit";
import { RouteAuthGuard } from "./RouteAuthGuard";
//import { AllUserType, UserType } from "../../types";

export const RouterConfig: React.FC = () => {
    return (
        <>

            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route index element={<Home />} />
                        <Route path="*" element={<NotFound />} />
                        <Route path="/Login" element={<Login />} />
                        <Route path="/SignUp" element={<SignUp />} />
                        <Route path="/Logout" element={<Logout />} />

                        <Route path="/PrivatePage" element={
                            <RouteAuthGuard component={<PrivatePage />} redirect="/Login" />} />
                        <Route path="/User/Delete" element={
                            <RouteAuthGuard component={<DeleteUser />} redirect="/Login" />} />
                        <Route path="/User/ChangeName" element={
                            <RouteAuthGuard component={<ChangeName />} redirect="/Login" />} />
                        <Route path="/User/ChangePassword" element={
                            <RouteAuthGuard component={<ChangePassword />} redirect="/Login" />} />
                        <Route path="/User/Edit" element={
                            <RouteAuthGuard component={<EditUser />} redirect="/Login" />} />
                    </Route>
                </Routes>
            </BrowserRouter>

        </>
    );
}