import { useEffect, useState } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useUserContext } from "../../providers";

type SignUpFormInputs = {
    id: string;
    name: string;
    password: string;
    confirmPassword: string;
};

export const SignUp = () => {
    const { control, register, setError, handleSubmit, formState: { errors } } = useForm<SignUpFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const { user, login } = useUserContext();

    useEffect(() => {
        if (user) {
            navigate('/PrivatePage');
        }
    }, [])

    const checkUserIdDuplication = async (value: string) => {
        if (value == "") {
            return true;
        };

        try {
            // APIリクエストを送信
            const response = await axios.get(`/api/user/Exists?id=${value}`);
            if (response.data) {
                return false;
            } else {
                return true;
            }
        } catch (error: any) {
            if (error.response.data.name) {
                setError(error.response.data.name, { message: error.response.data.message });
            } else {
                setErrorMessage(error.response?.data?.message || 'エラーが発生しました。');
            }
            return true;
        }
    };

    const onSubmit: SubmitHandler<SignUpFormInputs> = async (data) => {
        if (data.password !== data.confirmPassword) {
            
            setError('confirmPassword', { type: 'manual', message: "新しいパスワードと一致させてください。" });
            setIsLoading(false);
            return;
        }

        try {
            await axios.post('/api/user/SignUp', data)
                .then(() => {
                    console.log("Sign Up Success");
                    login(data.id, data.password, () => navigate('/PrivatePage'));
                });

        } catch (error: any) {
            console.error(error);
            setErrorMessage(error.response.data.message || 'エラーが発生しました。');
        }
    };

    return (
        <div>
            <h2 className="pageTitle">Sign Up</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
                        <dt><label>Id</label></dt>
                        <dd>
                            <Controller
                                name="id"
                                control={control}
                                defaultValue=""
                                render={({ field }) => (
                                    <input
                                        {...field}
                                        {...register('id', {
                                            required: "必須です。",
                                            minLength: { value: 3, message: "IDは3文字以上です。" },
                                            maxLength: { value: 20, message: "IDは20文字以下です。" },
                                            validate: { checkUserIdDuplication }
                                        })}
                                        type="text"
                                        onBlur={
                                            async(e) => {
                                                // フォーカスが外れたときに重複確認を実行
                                                const result = await checkUserIdDuplication(e.target.value);
                                                if (result) {
                                                    setError('id', { type: 'manual', message: "" });
                                                } else {
                                                    setError('id', { type: 'manual', message: "このIDは使用されています。" }); 
                                                }
                                            }
                                        }
                                    />
                                )}
                            />
                            <span>{errors.id?.message}</span>
                        </dd>
                        <dt><label>Name</label></dt>
                        <dd>
                            <input type="text" {...register('name', {
                                required: "必須です。",
                                minLength: { value: 3, message: "名前は3文字以上です。" },
                                maxLength: { value: 30, message: "名前は30文字以下です。" },
                            })} />
                            <span>{errors.name?.message}</span>
                        </dd>
                        <dt><label>Password</label></dt>
                        <dd>
                            <input type="password" {...register('password', {
                                required: "必須です。",
                                minLength: { value: 8, message: "パスワードは8文字以上です。" },
                                maxLength: { value: 16, message: "パスワードは16文字以下です。" },
                            })} />
                            <span>{errors.password?.message}</span>
                        </dd>
                        <dt><label>Confirm Password</label></dt>
                        <dd>
                            <input type="password" {...register('confirmPassword', {
                                required: "必須です。",
                                minLength: { value: 8, message: "パスワードは8文字以上です。" },
                                maxLength: { value: 16, message: "パスワードは16文字以下です。" },
                            })} />
                            <span>{errors.confirmPassword?.message}</span>
                        </dd>
                    </dl>
                    <div className="formErrorMassage">{errorMessage && <p>{errorMessage}</p>}</div>
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Sign Up</button></div>
                </form>      
            </div>
        </div>
    );
};

