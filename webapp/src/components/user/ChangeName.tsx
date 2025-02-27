import { useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useUserContext } from "../../providers";

type ChangeNameFormInputs = {
    name: string;
};

export const ChangeName = () => {
    const { register, handleSubmit, formState: { errors } } = useForm<ChangeNameFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const { fetchUser } = useUserContext();

    const onSubmit: SubmitHandler<ChangeNameFormInputs> = async (data) => {
        setIsLoading(true);
        try {
            // �T�[�o�[�Ƀ��O�C�����N�G�X�g�𑗐M
            await axios.post('/api/user/ChangeName', data).then(() => {
                fetchUser();
                navigate('/PrivatePage');
            });
        } catch (error: any) {
            // �G���[���b�Z�[�W��\��
            console.error(error);
            setErrorMessage(error.response.data.message || '�G���[���������܂����B');
        } finally {
            setIsLoading(false);
        }
    };


    return (
        <div>
            <h2 className="pageTitle">Change Name</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
                        <dt><label>Name</label></dt>
                        <dd>
                            <input type="text" {...register('name', {
                                required: "�K�{�ł��B",
                                minLength: { value: 3, message: "���O��3�����ȏ�ł��B" },
                                maxLength: { value: 30, message: "���O��30�����ȉ��ł��B" },
                            })} />
                            <span>{errors.name?.message}</span>
                        </dd>
                    </dl>
                    <div className="formErrorMassage">{errorMessage && <p>{errorMessage}</p>}</div>
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Change</button></div>
                </form>
            </div>
        </div>
    );
}

export default ChangeName;