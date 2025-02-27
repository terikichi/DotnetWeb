import axios from 'axios';
import React, { useEffect, useState, createContext, useContext } from 'react';
import { User, numberToUserType, numberToUserState } from '../types';

type UserContextType = {
    user: User | null;
    fetchUser: () => void;
    login: (id: string, password: string, callback: () => void) => void;
    logout: (callback: () => void) => void;
    isFetching: boolean;
    error: string | null;
};

const UserContext = createContext<UserContextType>({} as UserContextType);

export const useUserContext = (): UserContextType => {
    return useContext<UserContextType>(UserContext);
};

type Props = {
    children: React.ReactNode;
};

type LoginRequest = {
    id: string;
    password: string;
};

export const UserProvider = (props: Props) => {
    const [isFetching, setIsFetching] = useState(true);
    const [user, setUser] = useState<User | null>(null);
    const [error, setError] = useState<string | null>(null);
    
    const fetchUser = async () => {
        await axios.get('/api/auth/RequestUser')
            .then((response) => {
                const userData: User = {
                    id: response.data.id,
                    name: response.data.name,
                    userType: numberToUserType(response.data.type),
                    userState: numberToUserState(response.data.state)
                };
                setUser(userData);
            })
            .catch((error) => {
                setError(error);
                console.error(error);
            }).finally(() => {
                setIsFetching(false);
            });
    }

    useEffect(() => {
        fetchUser();
    }, [])

    const login = async (_id: string, _password: string, callback: () => void) => {
        const userData: LoginRequest = {
            id: _id,
            password: _password
        }

        try {
            await axios.post('/api/auth/Login', userData)
                .then(() => {
                    fetchUser()
                        .then(() => callback());
                });
        } catch (error: any) {
            throw error;
        }
    }

    const logout = (callback: () => void) => {
        setUser(null);
        callback();
    }

    const value: UserContextType = { user, fetchUser, login, logout, isFetching, error };
    return (
        <UserContext.Provider value={value}>
            {props.children}
        </UserContext.Provider>
    );
};