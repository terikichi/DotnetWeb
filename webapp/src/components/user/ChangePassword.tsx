import { useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

type ChangePasswordFormInputs = {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
};

export const ChangePassword = () => {
    const { register, setError, handleSubmit, formState: { errors } } = useForm<ChangePasswordFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();


    const onSubmit: SubmitHandler<ChangePasswordFormInputs> = async (data) => {
        setIsLoading(true);

        if (data.currentPassword == data.newPassword) {
            setError('newPassword', { type: 'manual', message: "現在のパスワードと同じです。" });
            setIsLoading(false);
            return;
        }

        if (data.newPassword !== data.confirmPassword) {
            setError('confirmPassword', { type: 'manual', message: "新しいパスワードと一致させてください。" });
            setIsLoading(false);
            return;
        }

        try {
            await axios.post('/api/user/ChangePassword', data)
                .then((response) => {
                    if (response.data) {
                        navigate('/PrivatePage');
                    }
                });
        } catch (error: any) {
            // エラーメッセージを表示
            console.error(error);
            if (error.response.data.name) {
                setError(error.response.data.name, { message: error.response.data.message });
            } else {
                setErrorMessage(error.response?.data?.message || 'エラーが発生しました。');
            }
        } finally {
            setIsLoading(false);
        }
    };


    return (
        <div>
            <h2 className="pageTitle">ChangePassword</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
                        <dt><label>Current Password</label></dt>
                        <dd>
                            <input type="password" {...register('currentPassword', {
                                required: "必須です。",
                                minLength: { value: 8, message: "パスワードは8文字以上です。" },
                                maxLength: { value: 16, message: "パスワードは16文字以下です。" }
                            })} />
                            <span>{errors.currentPassword?.message}</span>
                        </dd>
                        <dt><label>New Password</label></dt>
                        <dd>

                            <input
                                {...register('newPassword', {
                                    required: "必須です。",
                                    minLength: { value: 8, message: "パスワードは8文字以上です。" },
                                    maxLength: { value: 16, message: "パスワードは16文字以下です。" }
                                })}
                                type="password"
                            />
                            <span>{errors.newPassword?.message}</span>
                        </dd>
                        <dt><label>Confirm Password</label></dt>
                        <dd>
                            <input
                                {...register('confirmPassword', {
                                    required: "必須です。",
                                    minLength: { value: 8, message: "パスワードは8文字以上です。" },
                                    maxLength: { value: 16, message: "パスワードは16文字以下です。" }
                                })}
                                type="password"
                            />
                            <span>{errors.confirmPassword?.message}</span>
                        </dd>
                    </dl>
                    <div className="formErrorMassage">{errorMessage && <p>{errorMessage}</p>}</div>
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Change</button></div>
                </form>
            </div>
        </div>
    );
}

export default ChangePassword;