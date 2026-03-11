datasg segment

old_cs dw ?
old_ip dw ?

tick dw 0
speed dw 18
atr db 14
msg db 'X-Y'
MSG_LEN equ $-msg
POS_Y equ 14
POS_X equ 40

datasg ends


stacksg segment stack
db 256 dup(?)
stacksg ends


codesg segment
assume cs:codesg, ds:datasg, ss:stacksg


new_1c proc far

push ax
push bx
push ds
push es

mov ax, datasg
mov ds, ax

inc tick

mov ax, 40h
mov es, ax

mov ax, es:[1Ch]
mov bx, es:[1Ah]

cmp bx, ax
jne save_sym
jmp back


save_sym:
mov al, es:[bx]
mov es:[1Ch], bx
mov msg[0], al
cmp al, '1'
jne check2
mov speed, 36
jmp back

check2:
cmp al, '2'
jne check3
mov speed, 18
jmp back

check3:
cmp al, '3'
jne back
mov speed, 9

back:

pop es
pop ds
pop bx
pop ax

iret

new_1c endp



start:
mov ax, datasg
mov ds, ax
mov ah,35h
mov al,1Ch
int 21h
mov old_ip, bx
mov old_cs, es
push ds
mov dx, offset new_1c
mov ax, seg new_1c
mov ds, ax
mov ah,25h
mov al,1Ch
int 21h
pop ds
mov ax, datasg
mov es, ax


main_loop:
cmp msg[0], 30h
je quit
mov ax, speed
cmp tick, ax
jl check_for_changes
mov tick, 0
mov al, atr
inc al
cmp al, 15
jle save_attr
mov al,1

save_attr:
mov atr, al
jmp show_msg


check_for_changes:
cmp bh, msg
je main_loop


show_msg:
xor bh, bh
mov ah,13h
mov al,0
mov dh,POS_Y
mov dl,POS_X
lea bp,msg
mov bl,atr
mov cx,MSG_LEN
int 10h
mov bh,msg[0]
jmp main_loop

quit:
push ds
mov dx, old_ip
mov ax, old_cs
mov ds, ax
mov ah,25h
mov al,1Ch
int 21h
pop ds
mov ax,4C00h
int 21h

codesg ends
end start
